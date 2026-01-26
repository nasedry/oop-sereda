using System;

namespace lab21
{
    public interface IShippingStrategy
    {
        decimal CalculateCost(decimal distance, decimal weight);
    }

    public class StandardShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            return distance * 1.5m + weight * 0.5m;
        }
    }

    public class ExpressShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            return distance * 2.5m + weight * 1.0m + 50;
        }
    }

    public class InternationalShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            decimal baseCost = distance * 5.0m + weight * 2.0m;
            return baseCost + baseCost * 0.15m;
        }
    }

    public class NightShippingStrategy : IShippingStrategy
    {
        public decimal CalculateCost(decimal distance, decimal weight)
        {
            decimal standard = distance * 1.5m + weight * 0.5m;
            return standard + 30;
        }
    }

    public static class ShippingStrategyFactory
    {
        public static IShippingStrategy CreateStrategy(string type)
        {
            return type.ToLower() switch
            {
                "standard" => new StandardShippingStrategy(),
                "express" => new ExpressShippingStrategy(),
                "international" => new InternationalShippingStrategy(),
                "night" => new NightShippingStrategy(),
                _ => throw new ArgumentException("Error")
            };
        }
    }

    public class DeliveryService
    {
        public decimal CalculateDeliveryCost(decimal distance, decimal weight, IShippingStrategy strategy)
        {
            return strategy.CalculateCost(distance, weight);
        }
    }

    public interface IGymPassStrategy
    {
        decimal CalculatePrice(int hours, bool sauna, bool pool);
    }

    public class MorningPass : IGymPassStrategy
    {
        public decimal CalculatePrice(int hours, bool sauna, bool pool)
        {
            decimal price = hours * 50;
            if (sauna) price += 100;
            if (pool) price += 80;
            return price * 0.8m;
        }
    }

    public class DayPass : IGymPassStrategy
    {
        public decimal CalculatePrice(int hours, bool sauna, bool pool)
        {
            decimal price = hours * 70;
            if (sauna) price += 120;
            if (pool) price += 100;
            return price;
        }
    }

    public class FullPass : IGymPassStrategy
    {
        public decimal CalculatePrice(int hours, bool sauna, bool pool)
        {
            decimal price = hours * 90;
            if (sauna) price += 150;
            if (pool) price += 120;
            return price * 1.2m;
        }
    }

    public static class GymPassFactory
    {
        public static IGymPassStrategy CreatePass(string type)
        {
            return type.ToLower() switch
            {
                "morning" => new MorningPass(),
                "day" => new DayPass(),
                "full" => new FullPass(),
                _ => throw new ArgumentException("Error")
            };
        }
    }

    class Program
    {
        static void Main()
        {
            IShippingStrategy strategy;
            string type;
            while (true)
            {
                Console.Write("Type (standard / express / international / night): ");
                type = Console.ReadLine();
                try
                {
                    strategy = ShippingStrategyFactory.CreateStrategy(type);
                    break;
                }
                catch
                {
                    Console.WriteLine("Невідомий тип доставки, спробуйте ще раз.");
                }
            }

            decimal distance;
            while (true)
            {
                Console.Write("Distance: ");
                if (decimal.TryParse(Console.ReadLine(), out distance)) break;
                Console.WriteLine("Введіть число!");
            }

            decimal weight;
            while (true)
            {
                Console.Write("Weight: ");
                if (decimal.TryParse(Console.ReadLine(), out weight)) break;
                Console.WriteLine("Введіть число!");
            }

            var service = new DeliveryService();
            Console.WriteLine("Вартість доставки: " + service.CalculateDeliveryCost(distance, weight, strategy));

            IGymPassStrategy passStrategy;
            string passType;
            while (true)
            {
                Console.Write("Pass (morning / day / full): ");
                passType = Console.ReadLine();
                try
                {
                    passStrategy = GymPassFactory.CreatePass(passType);
                    break;
                }
                catch
                {
                    Console.WriteLine("Невідомий тип абонемента, спробуйте ще раз.");
                }
            }

            int hours;
            while (true)
            {
                Console.Write("Hours: ");
                if (int.TryParse(Console.ReadLine(), out hours)) break;
                Console.WriteLine("Введіть число!");
            }

            bool sauna;
            while (true)
            {
                Console.Write("Sauna (true / false): ");
                if (bool.TryParse(Console.ReadLine(), out sauna)) break;
                Console.WriteLine("Введіть true або false!");
            }

            bool pool;
            while (true)
            {
                Console.Write("Pool (true / false): ");
                if (bool.TryParse(Console.ReadLine(), out pool)) break;
                Console.WriteLine("Введіть true або false!");
            }

            Console.WriteLine("Вартість абонемента: " + passStrategy.CalculatePrice(hours, sauna, pool));
        }
    }
}
