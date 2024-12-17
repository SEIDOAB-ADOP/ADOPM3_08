using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADOPM3_08_05
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var random = new Random();
            IEnumerable<Task<int>> tasks = Enumerable.Range(1, 5).Select(n => Task.Run(async () =>
            {
                int mysleep = random.Next(1000, 5000);
                Console.WriteLine($"I'm task {n} and I am sleeping {mysleep}ms");
                await Task.Delay(mysleep);
                return n;
            }));


            Task<Task<int>> whenAnyTask = Task.WhenAny(tasks);
            Task<int> completedTask = await whenAnyTask;
            Console.WriteLine("The winner is: task " + completedTask.Result);

            Task.WhenAll(tasks).Wait();
            Console.WriteLine("All tasks finished!");
        }
    }
}
