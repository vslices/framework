using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public static class TodoPrograms
{
    public static Free<TodoAlgebra, Option<Todo>> Create(Todo point) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(point.Id)
        from created in current.Match(
            Some: static _ =>
                Free.pure<TodoAlgebra, Option<Todo>>(Option<Todo>.None),
            None: () =>
                from _ in PointWriter.write<TodoAlgebra, Todo>(point)
                select Some(point))
        select created;

    public static Free<TodoAlgebra, Option<Todo>> Read(TodoId id) =>
        PointReader.read<TodoAlgebra, Todo, TodoId>(id);

    public static Free<TodoAlgebra, Option<Todo>> Update(Todo point) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(point.Id)
        from updated in current.Match(
            Some: _ =>
                from __ in PointWriter.write<TodoAlgebra, Todo>(point)
                from value in PointReader.read<TodoAlgebra, Todo, TodoId>(point.Id)
                select value,
            None: static () =>
                Free.pure<TodoAlgebra, Option<Todo>>(Option<Todo>.None))
        select updated;

    public static Free<TodoAlgebra, Option<Todo>> Delete(TodoId id) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(id)
        from deleted in current.Match(
            Some: point =>
                from _ in PointRemover.remove<TodoAlgebra, Todo, TodoId>(id)
                select Some(point),
            None: static () =>
                Free.pure<TodoAlgebra, Option<Todo>>(Option<Todo>.None))
        select deleted;
}
