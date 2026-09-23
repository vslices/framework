using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;

namespace SampleWorkflow.Work;

/// <summary>
/// Experimental UpdateTodo variant that drives the same semantic intention through
/// a deliberately deep, allocation-free-by-shape lowering hierarchy.
/// </summary>
public sealed class UpdateTodoMicroOptimized :
    Feature<
        UpdateTodoMicroOptimized,
        UpdateTodoMicroOptimized.Algebra,
        UpdateTodoMicroOptimized.Request,
        UpdateTodoMicroOptimized.Response>
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
    public readonly record struct Step(Substep Substep);
    public readonly record struct Substep(Microstep Microstep);
    public readonly record struct Microstep(Nanostep Nanostep);
    public readonly record struct Nanostep(Picostep Picostep);

    public readonly record struct Picostep(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record ExecuteLinePart<A>(
        Line Line,
        Func<Todo, bool> SatisfiedBy,
        Func<Todo, Fin<Todo>> Evolve,
        Func<Either<Error, Option<Todo>>, A> Next)
        : WorkPart<A>;

    public sealed class Algebra : Functor<Algebra>
    {
        public static K<Algebra, Either<Error, Option<Todo>>> Execute(
            Line line,
            Func<Todo, bool> satisfiedBy,
            Func<Todo, Fin<Todo>> evolve) =>
            new ExecuteLinePart<Either<Error, Option<Todo>>>(
                line,
                satisfiedBy,
                evolve,
                static result => result);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ExecuteLinePart<A>(
                    var line,
                    var satisfiedBy,
                    var evolve,
                    var next) =>
                    new ExecuteLinePart<B>(
                        line,
                        satisfiedBy,
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
                            new Substep(
                                new Microstep(
                                    new Nanostep(
                                        new Picostep(
                                            request.Id,
                                            request.Detail,
                                            request.Completed))))))));

        Func<Todo, bool> satisfiedBy =
            todo =>
                todo.Detail == request.Detail &&
                todo.Completed == request.Completed;

        Func<Todo, Fin<Todo>> evolve =
            todo => todo.Update(
                state => state with
                {
                    Detail = request.Detail,
                    Completed = request.Completed
                });

        return
            from result in Free.lift(
                Algebra.Execute(
                    line,
                    satisfiedBy,
                    evolve))
            select new Response(result);
    }
}
