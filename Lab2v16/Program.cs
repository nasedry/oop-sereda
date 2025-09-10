using System;

public class Currency
{
    private double amount;

    public Currency(double amount)
    {
        this.amount = amount;
    }

    public double Amount
    {
        get => amount;
        set => amount = value;
    }

    public double InUSD => amount / 40.0;
    public double InEUR => amount / 43.0;

    public double this[string currency]
    {
        get
        {
            return currency switch
            {
                "USD" => InUSD,
                "EUR" => InEUR,
                "UAH" => Amount,
                _ => throw new ArgumentException("Unknown currency")
            };
        }
    }

    public static Currency operator +(Currency a, Currency b)
    {
        return new Currency(a.amount + b.amount);
    }

    public static Currency operator -(Currency a, Currency b)
    {
        return new Currency(a.amount - b.amount);
    }

    public static bool operator ==(Currency a, Currency b)
    {
        return a.amount == b.amount;
    }

    public static bool operator !=(Currency a, Currency b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Currency other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        return amount.GetHashCode();
    }

    public override string ToString()
    {
        return $"{amount} UAH";
    }
}
class Program
{
    static void Main()
    {
        Currency c1 = new Currency(1000);
        Currency c2 = new Currency(500);

        Console.WriteLine(c1);
        Console.WriteLine(c2);

        Console.WriteLine($"{c1.InUSD:F2}");
        Console.WriteLine($"{c1.InEUR:F2}");

        Console.WriteLine($"{c2["USD"]:F2}");
        Console.WriteLine($"{c2["UAH"]:F2}");

        Currency sum = c1 + c2;
        Console.WriteLine(sum);

        Currency diff = c1 - c2;
        Console.WriteLine(diff);

        Console.WriteLine(c1 == c2);
    }
}
