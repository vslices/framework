using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;

namespace SampleWorkflow.Work;

/// <summary>
/// Experimental UpdateTodo variant that keeps the semantic update owned by Work
/// while allowing Grounding to fuse read/evolve/write realization.
/// </summary>
public sealed class UpdateTodoOptimized :
    Feature<
        UpdateTodoOptimized,
        UpdateTodoOptimized.Algebra,
        UpdateTodoOptimized.Request,
        UpdateTodoOptimized.Response>
{
    public sealed record Request(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);

    public readonly record struct Line(Process Process);
    public readonly record struct Process(Flow Flow);
    public readonly record struct Flow(Step Step);

    public readonly record struct Step(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record ExecuteLinePart<A>(
        Line Line,
        Func<Todo, Fin<Todo>> Evolve,
        Func<Either<Error, Option<Todo>>, A> Next)
        : WorkPart<A>;

    public sealed class Algebra : Functor<Algebra>
    {
        public static K<Algebra, Either<Error, Option<Todo>>> Execute(
            Line line,
            Func<Todo, Fin<Todo>> evolve) =>
            new ExecuteLinePart<Either<Error, Option<Todo>>>(
                line,
                evolve,
                static result => result);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ExecuteLinePart<A>(var line, var evolve, var next) =>
                    new ExecuteLinePart<B>(
                        line,
                        evolve,
                        result => f(next(result))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request)
    {
        var line =
            new Line(
                new Process(
                    new Flow(
                        new Step(
                            request.Id,
                            request.Detail,
                            request.Completed))));

        Func<Todo, Fin<Todo>> evolve =
            todo => todo.Update(
                state => state with
                {
                    Detail = request.Detail,
                    Completed = request.Completed
                });

        return
            from result in Free.lift(Algebra.Execute(line, evolve))
            select new Response(result);
    }
}
