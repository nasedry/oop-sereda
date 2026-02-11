using System;
using System.Collections.Generic;

namespace lab24
{
    public interface INumericOperationStrategy
    {
        double Execute(double value);
        string Name { get; }
    }

    public class SquareOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Square";
        public double Execute(double value) => value * value;
    }

    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Cube";
        public double Execute(double value) => value * value * value;
    }

    public class SquareRootOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Square Root";
        public double Execute(double value) => Math.Sqrt(value);
    }

    public class NumericProcessor
    {
        private INumericOperationStrategy _strategy;

        public NumericProcessor(INumericOperationStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(INumericOperationStrategy strategy)
        {
            _strategy = strategy;
        }

        public double Process(double input)
        {
            return _strategy.Execute(input);
        }

        public string GetOperationName()
        {
            return _strategy.Name;
        }
    }

    public class ResultPublisher
    {
        public event Action<double, string> ResultCalculated;

        public void PublishResult(double result, string operationName)
        {
            ResultCalculated?.Invoke(result, operationName);
        }
    }

    public class ConsoleLoggerObserver
    {
        public void Subscribe(ResultPublisher publisher)
        {
            publisher.ResultCalculated += OnResultCalculated;
        }

        private void OnResultCalculated(double result, string operationName)
        {
            Console.WriteLine($"Operation: {operationName} | Result: {result}");
        }
    }

    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new List<string>();

        public void Subscribe(ResultPublisher publisher)
        {
            publisher.ResultCalculated += OnResultCalculated;
        }

        private void OnResultCalculated(double result, string operationName)
        {
            History.Add($"Operation: {operationName} | Result: {result}");
        }

        public void PrintHistory()
        {
            Console.WriteLine("History:");
            foreach (var item in History)
                Console.WriteLine(item);
        }
    }

    public class ThresholdNotifierObserver
    {
        private readonly double _threshold;

        public ThresholdNotifierObserver(double threshold)
        {
            _threshold = threshold;
        }

        public void Subscribe(ResultPublisher publisher)
        {
            publisher.ResultCalculated += OnResultCalculated;
        }

        private void OnResultCalculated(double result, string operationName)
        {
            if (result > _threshold)
            {
                Console.WriteLine($"Result {result} exceeded threshold {_threshold}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var processor = new NumericProcessor(new SquareOperationStrategy());
            var publisher = new ResultPublisher();

            var consoleObserver = new ConsoleLoggerObserver();
            var historyObserver = new HistoryLoggerObserver();
            var thresholdObserver = new ThresholdNotifierObserver(50);

            consoleObserver.Subscribe(publisher);
            historyObserver.Subscribe(publisher);
            thresholdObserver.Subscribe(publisher);

            double[] numbers = { 4, 9, 16 };

            foreach (var number in numbers)
            {
                double result = processor.Process(number);
                publisher.PublishResult(result, processor.GetOperationName());
            }

            processor.SetStrategy(new CubeOperationStrategy());

            foreach (var number in numbers)
            {
                double result = processor.Process(number);
                publisher.PublishResult(result, processor.GetOperationName());
            }

            processor.SetStrategy(new SquareRootOperationStrategy());

            foreach (var number in numbers)
            {
                double result = processor.Process(number);
                publisher.PublishResult(result, processor.GetOperationName());
            }

            historyObserver.PrintHistory();
        }
    }
}
