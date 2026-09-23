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
    public readonly record struct Request(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public readonly record struct Response(
        Either<Error, Option<Todo>> Todo);

    public readonly record struct Line(Process Process);
    public readonly record struct Process(Flow Flow);
    public readonly record struct Flow(Step Step);
    public readonly record struct Step(Substep Substep);
    public readonly record struct Substep(Microstep Microstep);
    public readonly record struct Microstep(Nanostep Nanostep);
    public readonly record struct Nanostep(Picostep Picostep);
    public readonly record struct Picostep(Femtostep Femtostep);
    public readonly record struct Femtostep(Attostep Attostep);
    public readonly record struct Attostep(Zeptostep Zeptostep);
    public readonly record struct Zeptostep(Yoctostep Yoctostep);
    public readonly record struct Yoctostep(Rontostep Rontostep);
    public readonly record struct Rontostep(Quectostep Quectostep);

    public readonly record struct Quectostep(
        TodoId Id,
        TodoDetail Detail,
        bool Completed);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record ExecuteLinePart<A>(
        Line Line,
        Func<Response, A> Next)
        : WorkPart<A>;

    public sealed class Algebra : Functor<Algebra>
    {
        public static K<Algebra, Response> Execute(Line line) =>
            new ExecuteLinePart<Response>(
                line,
                static response => response);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ExecuteLinePart<A>(var line, var next) =>
                    new ExecuteLinePart<B>(
                        line,
                        response => f(next(response))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request) =>
        Free.lift(
            Algebra.Execute(
                ToLine(request)));

    public static bool SatisfiedBy(
        Todo todo,
        Quectostep step) =>
        todo.Detail == step.Detail &&
        todo.Completed == step.Completed;

    public static Fin<Todo> Evolve(
        Todo todo,
        Quectostep step) =>
        todo.Update(
            state => state with
            {
                Detail = step.Detail,
                Completed = step.Completed
            });

    private static Line ToLine(Request request) =>
        new(
            new Process(
                new Flow(
                    new Step(
                        new Substep(
                            new Microstep(
                                new Nanostep(
                                    new Picostep(
                                        new Femtostep(
                                            new Attostep(
                                                new Zeptostep(
                                                    new Yoctostep(
                                                        new Rontostep(
                                                            new Quectostep(
                                                                request.Id,
                                                                request.Detail,
                                                                request.Completed))))))))))))));
}
