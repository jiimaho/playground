using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Playgound.Web.Threadpool.SimpleExhaustion.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Making_100_calls_will_not_exhaust_the_thread_pool()
        {
            ServicePointManager.DefaultConnectionLimit = Int32.MaxValue;
            
            using var httpClient = new HttpClient();
            
            var tasks = new List<Task>();

            for (int i = 0; i < 2000; i++)
            {
                var call = httpClient.GetAsync("https://localhost:5001/exhaust");
                    
                Console.WriteLine(i);
                tasks.Add(call);
            }

            await Task.WhenAll(tasks);
            
            Console.WriteLine("");
        }
    }
}
