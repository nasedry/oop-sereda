using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("IndependentWork12 - PLINQ experiments");
        int[] sizes = new[] { 1_000_000, 5_000_000, 10_000_000 };
        string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "results.csv");
        using (var csv = new StreamWriter(csvPath, false))
        {
            csv.WriteLine("Test,Size,Mode,Run,ElapsedMs");
        }
        foreach (var n in sizes)
        {
            Console.WriteLine($"Generating {n:N0} doubles...");
            var data = GenerateRandomDoubles(n, 12345);
            Console.WriteLine("Done");
            Func<double, double> heavySqrt = x =>
            {
                double y = x;
                for (int i = 0; i < 50; i++) y = Math.Sqrt(y + i);
                return y;
            };
            Func<double, double> primeLike = x =>
            {
                int v = Math.Abs((int)(x * 1000)) | 1;
                return IsPrime(v) ? 1.0 : 0.0;
            };
            Console.WriteLine("Benchmark: heavySqrt");
            RunBenchmark("heavySqrt", data, heavySqrt, 3, csvPath);
            Console.WriteLine("Benchmark: primeLike");
            RunBenchmark("primeLike", data, primeLike, 3, csvPath);
            Console.WriteLine("================================================");
        }
        Console.WriteLine("Running side-effects demonstration");
        SideEffectsDemo();
        Console.WriteLine($"All done. Results saved to {Path.Combine(Directory.GetCurrentDirectory(), "results.csv")}");
    }

    static List<double> GenerateRandomDoubles(int count, int seed)
    {
        var rand = new Random(seed);
        var list = new List<double>(count);
        for (int i = 0; i < count; i++) list.Add(rand.NextDouble() * 100_000 + 1.0);
        return list;
    }

    static void RunBenchmark(string testName, List<double> data, Func<double, double> op, int repeats, string csvPath)
    {
        var warm = data.Take(1000).Select(op).ToArray();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Thread.Sleep(50);
        var linqTimes = new List<long>();
        for (int r = 0; r < repeats; r++)
        {
            var sw = Stopwatch.StartNew();
            var res = data.Where(x => x > 0).Select(op).ToArray();
            sw.Stop();
            linqTimes.Add(sw.ElapsedMilliseconds);
            Console.WriteLine($"LINQ run {r + 1}: {sw.ElapsedMilliseconds} ms");
            AppendCsv(testName, data.Count, "LINQ", r + 1, sw.ElapsedMilliseconds, csvPath);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(50);
        }
        var plinqTimes = new List<long>();
        for (int r = 0; r < repeats; r++)
        {
            var sw = Stopwatch.StartNew();
            var res = data.AsParallel().Where(x => x > 0).Select(op).ToArray();
            sw.Stop();
            plinqTimes.Add(sw.ElapsedMilliseconds);
            Console.WriteLine($"PLINQ run {r + 1}: {sw.ElapsedMilliseconds} ms");
            AppendCsv(testName, data.Count, "PLINQ", r + 1, sw.ElapsedMilliseconds, csvPath);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(50);
        }
        double avgL = linqTimes.Average();
        double avgP = plinqTimes.Average();
        Console.WriteLine($"Average LINQ: {avgL} ms | Average PLINQ: {avgP} ms");
        Console.WriteLine(avgP < avgL ? $"PLINQ faster by {avgL - avgP} ms" : $"PLINQ slower or equal by {avgP - avgL} ms");
    }

    static void AppendCsv(string test, int size, string mode, int run, long elapsedMs, string path)
    {
        using (var sw = new StreamWriter(path, true))
        {
            sw.WriteLine($"{test},{size},{mode},{run},{elapsedMs}");
        }
    }

    static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0) return n == 2;
        int r = (int)Math.Sqrt(n);
        for (int i = 3; i <= r; i += 2) if (n % i == 0) return false;
        return true;
    }

    static void SideEffectsDemo()
    {
        const int N = 1_000_000;
        var data = Enumerable.Range(0, N).ToList();
        int unsafeCounter = 0;
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0) unsafeCounter++;
        });
        Console.WriteLine($"Unsafe counter expected {N / 2}, got {unsafeCounter}");
        int lockedCounter = 0;
        var lockObj = new object();
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0)
            {
                lock (lockObj) { lockedCounter++; }
            }
        });
        Console.WriteLine($"Locked counter expected {N / 2}, got {lockedCounter}");
        int interlockedCounter = 0;
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0) System.Threading.Interlocked.Increment(ref interlockedCounter);
        });
        Console.WriteLine($"Interlocked counter expected {N / 2}, got {interlockedCounter}");
        var bag = new ConcurrentBag<int>();
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0) bag.Add(x);
        });
        Console.WriteLine($"ConcurrentBag count expected {N / 2}, got {bag.Count}");
    }
}
