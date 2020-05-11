using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestClient
{
    class Program
    {
        private const int TotalRequestsCount = 100;
        private const int StartDelay = 20 * 1000; // seconds

        static void Main(string[] args)
        {

            Thread.Sleep(StartDelay);

            var tasks = new List<Task>();

            for (int i = 1; i <= TotalRequestsCount; i++)
            {
                var index = i;
                tasks.Add(Task.Run(() => MakeRequest(index)));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                Console.WriteLine();
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var successCount = tasks.Count(task => task.IsCompletedSuccessfully);
            Console.WriteLine();
            Console.WriteLine($"Completed successfully {successCount} from {TotalRequestsCount}");

            Console.WriteLine();
            Thread.Sleep(1000);
            MakeSimpleRequest();

            Console.ReadLine();
        }

        private static async Task MakeRequest(int index)
        {
            var formattedIndex = index.ToString().PadLeft(3);

            using (var client = new HttpClient())
            {
                Console.WriteLine($"Start request : {formattedIndex}");
                var url = @"http://localhost:5000/data/" + index;

                var timer = new Stopwatch();
                timer.Start();

                try
                {
                    var response =  await client.GetAsync(new Uri(url)).ConfigureAwait(false);
                    var result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw new Exception($"{e.Message} index : {index}");
                }

                timer.Stop();
                var timeTaken = timer.Elapsed;
                var time = "Duration - " + timeTaken.ToString(@"m\:ss\.ff");
                Console.WriteLine($"End request : {formattedIndex} {time} (min:sec)");
            }
        }

        private static async void MakeSimpleRequest()
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine($"Start simple request");
                var url = @"http://localhost:5000/data/";

                var response = await client.GetAsync(new Uri(url)).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                Console.WriteLine($"End simple request");
            }
        }
    }
}
