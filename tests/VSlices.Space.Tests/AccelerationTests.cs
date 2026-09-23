using System.Numerics;
using VSlices.Space.Quantities;
using VSlices.Space.Quantities.Abstract;
using static VSlices.Space.Conversions;
using static VSlices.Space.Quantities.PowerOperations;

namespace VSlices.Space.Tests;

public class AccelerationTests
{
    [Fact]
    public void duration_square_preserves_the_primitive_duration_basis()
    {
        var duration = new ProbeDuration<Hours, decimal>(2m);

        Power<M.Duration, N2, Hours, decimal> squared = square(duration);

        Assert.Equal(4m, squared.Value);
        Assert.IsAssignableFrom<Q<M.Pow<M.Duration, N2>, Hours, decimal>>(squared);
    }

    [Fact]
    public void length_divided_by_squared_duration_materializes_the_reduced_div_shape()
    {
        var distance = new ProbeLength<Kilometers, decimal>(120m);
        var duration = new ProbeDuration<Hours, decimal>(2m);

        Quotient<
            M.Length,
            Kilometers,
            M.Pow<M.Duration, N2>,
            Hours,
            decimal> structural = distance / square(duration);

        Assert.Equal(30m, structural.Value);
    }

    [Fact]
    public void speed_divided_by_duration_reduces_nested_division_and_aligns_duration_basis()
    {
        var distance = new ProbeLength<Kilometers, decimal>(120m);
        var elapsed = new ProbeDuration<Hours, decimal>(2m);
        var interval = new ProbeDuration<Minutes, decimal>(30m);
        var semanticSpeed = speed(distance / elapsed);

        Quotient<
            M.Length,
            Kilometers,
            M.Pow<M.Duration, N2>,
            Hours,
            decimal> structural = semanticSpeed / interval;

        Assert.Equal(120m, structural.Value);
    }

    [Fact]
    public void acceleration_is_explicitly_established_from_length_per_duration_squared()
    {
        var distance = new ProbeLength<Kilometers, decimal>(120m);
        var duration = new ProbeDuration<Hours, decimal>(2m);
        var structural = distance / square(duration);

        Acceleration<Kilometers, Hours, decimal> semantic = acceleration(structural);
        Quotient<M.Length, Kilometers, M.Pow<M.Duration, N2>, Hours, decimal> widened =
            WidenAcceleration(semantic);

        Assert.Equal(30m, semantic.Value);
        Assert.Equal(structural, widened);
    }

    [Fact]
    public void direct_and_reduced_nested_division_reach_the_same_acceleration_shape()
    {
        var distance = new ProbeLength<Kilometers, decimal>(120m);
        var twoHours = new ProbeDuration<Hours, decimal>(2m);
        var halfHour = new ProbeDuration<Minutes, decimal>(30m);
        var oneHour = new ProbeDuration<Hours, decimal>(1m);

        var direct = acceleration(distance / square(oneHour));
        var nested = acceleration(speed(distance / twoHours) / halfHour);

        Assert.Equal(120m, direct.Value);
        Assert.Equal(direct.Value, nested.Value);
        Assert.IsType<Acceleration<Kilometers, Hours, decimal>>(nested);
    }

    [Fact]
    public void acceleration_preserves_multiple_primitive_bases_without_forcing_q()
    {
        var accelerationType = typeof(Acceleration<Kilometers, Hours, decimal>);

        Assert.DoesNotContain(
            accelerationType.GetInterfaces(),
            candidate => candidate.IsGenericType &&
                         candidate.GetGenericTypeDefinition() == typeof(Q<,,>));

        var attribute = Assert.Single(
            accelerationType
                .GetCustomAttributes(typeof(AlgebraicSymbolAttribute), inherit: false)
                .Cast<AlgebraicSymbolAttribute>());

        Assert.Equal("acceleration", attribute.Symbol);
    }

    private static Quotient<M.Length, LENGTH_C, M.Pow<M.Duration, N2>, DURATION_C, T>
        WidenAcceleration<LENGTH_C, DURATION_C, T>(Acceleration<LENGTH_C, DURATION_C, T> acceleration)
        where LENGTH_C : Coordinate<M.Length>
        where DURATION_C : Coordinate<M.Duration>
        where T : INumber<T> =>
        acceleration.ToBase();

    private sealed class ProbeLength<C, T> : Length<ProbeLength<C, T>, C, T>
        where C : Coordinate<M.Length>
        where T : INumber<T>
    {
        public ProbeLength(T value) : base(value) { }
    }

    private sealed class ProbeDuration<C, T> : Duration<ProbeDuration<C, T>, C, T>
        where C : Coordinate<M.Duration>
        where T : INumber<T>
    {
        public ProbeDuration(T value) : base(value) { }
    }
}
