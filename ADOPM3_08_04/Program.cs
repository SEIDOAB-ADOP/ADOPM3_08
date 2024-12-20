﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ADOPM3_08_04
{
    internal class CPUBoundAsync
    {
        public Task DisplayPrimeCountsAsync(IProgress<(string, float)> onProgressReporting)
        {
            //Notice I can use async in Lambda Expression
            return Task.Run(async () =>
            {
                float completion = 0;
                for (int i = 0; i < 20; i++)
                {
                    int nrprimes = await GetPrimesCountAsync(i * 1000000 + 2, 1000000);
                    completion += 5;
                    onProgressReporting.Report(
                        ($"{nrprimes} primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1),
                        completion));
                }
            });
        }
        Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
               ParallelEnumerable.Range(start, count).Count(n =>
                 Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Invoking DisplayPrimeCountsAsync");

            //Define my progressReporter as an instance of Progress which implements IProgress
            var progressReporter = new Progress<(string, float)>(value =>
            {
                Console.WriteLine($"{value.Item2}%: {value.Item1}");
            });

            //Create and run the task, but passing the progressReporter as an argument
            await new CPUBoundAsync().DisplayPrimeCountsAsync(progressReporter);
        }
    }
}
