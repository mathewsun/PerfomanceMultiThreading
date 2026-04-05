using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PerfomanceMultiThreading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] threadCounts = { 1, 2, 4, 8, 12, 18, 24, 40, 50, 100, 200, 500, 1000, 5000, 10000, 50000, 100000 };
            long workSize = 50_000_000_0;

            foreach (int threadCount in threadCounts)
            {
                var stopwatch = Stopwatch.StartNew();

                ParallelOptions options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = threadCount
                };

                long total = 0;
                object locker = new object();

                Parallel.For(0, threadCount, options, i =>
                {
                    long localSum = 0;

                    for (int j = 0; j < workSize / threadCount; j++)
                    {
                        localSum += j % 3;
                    }

                    lock (locker)
                    {
                        total += localSum;
                    }
                });

                stopwatch.Stop();

                Console.WriteLine($"Потоки: {threadCount}, Время: {stopwatch.ElapsedMilliseconds} ms");
            }

            Console.WriteLine("Готово");
        }
    }
}
