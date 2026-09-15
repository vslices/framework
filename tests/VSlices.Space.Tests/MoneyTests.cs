using LanguageExt;
using VSlices.Space.Finance;

namespace VSlices.Space.Tests;

public class MoneyTests
{
    private readonly struct USD : Currency
    {
        public static string Code => "USD";
        public static string Name => "US Dollar";
        public static string Symbol => "$";
        public static int Decimals => 2;
    }

    private readonly struct CLP : Currency
    {
        public static string Code => "CLP";
        public static string Name => "Chilean Peso";
        public static string Symbol => "$";
        public static int Decimals => 0;
    }

    private readonly struct EUR : Currency
    {
        public static string Code => "EUR";
        public static string Name => "Euro";
        public static string Symbol => "€";
        public static int Decimals => 2;
    }

    [Fact]
    public void money_is_a_vector_space_within_one_currency()
    {
        var first = new Money<USD, decimal>(10m);
        var second = new Money<USD, decimal>(2m);

        var result = (first + second) * 3m - new Money<USD, decimal>(6m);

        Assert.Equal(30m, result.Amount);
        Assert.Equal(new Money<USD, decimal>(0m), Money<USD, decimal>.AdditiveIdentity);
        Assert.Contains(
            result.GetType().GetInterfaces(),
            candidate => candidate.IsGenericType &&
                         candidate.GetGenericTypeDefinition() == typeof(VectorSpace<,>));
    }

    [Fact]
    public void currencies_are_nominally_distinct_spaces_not_static_coordinates()
    {
        Assert.NotEqual(typeof(Money<USD, decimal>), typeof(Money<CLP, decimal>));
        Assert.Equal("USD", USD.Code);
        Assert.Equal("CLP", CLP.Code);
    }

    [Fact]
    public void exchange_rate_must_be_positive()
    {
        var zero = ExchangeRate<USD, CLP, decimal>.Create(0m);
        var negative = ExchangeRate<USD, CLP, decimal>.Create(-1m);

        Assert.True(zero.Match(Succ: _ => false, Fail: _ => true));
        Assert.True(negative.Match(Succ: _ => false, Fail: _ => true));
    }

    [Fact]
    public void exchange_rate_applies_runtime_conversion_evidence()
    {
        var rate = Success(ExchangeRate<USD, CLP, decimal>.Create(900m));
        var dollars = new Money<USD, decimal>(10m);

        var pesos = rate.Apply(dollars);

        Assert.Equal(9000m, pesos.Amount);
    }

    [Fact]
    public void exchange_rate_inversion_round_trips_money()
    {
        var rate = Success(ExchangeRate<USD, CLP, decimal>.Create(900m));
        var dollars = new Money<USD, decimal>(10m);

        var pesos = rate.Apply(dollars);
        var roundTrip = rate.Invert().Apply(pesos);

        Assert.Equal(dollars.Amount, roundTrip.Amount);
    }

    [Fact]
    public void exchange_rates_compose_through_matching_currency_spaces()
    {
        var usdToClp = Success(ExchangeRate<USD, CLP, decimal>.Create(900m));
        var clpToEur = Success(ExchangeRate<CLP, EUR, decimal>.Create(0.001m));

        var usdToEur = usdToClp.Then(clpToEur);
        var euros = usdToEur.Apply(new Money<USD, decimal>(10m));

        Assert.Equal(9m, euros.Amount);
        Assert.Equal(0.9m, usdToEur.Value);
    }

    private static A Success<A>(Fin<A> value) =>
        value.Match(
            Succ: result => result,
            Fail: error => throw new InvalidOperationException(error.ToString()));
}
