using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Playgound.SimpleAsyncAwait
{
    public class MyObject
    {
        public async Task<bool> Delay()
        {
            Console.WriteLine("Wheel: running for 15 seconds");


            var httpClient = new HttpClient();
            var deadlock = httpClient.GetAsync("https://localhost:5000").Result;

            Console.WriteLine("Wheel: 15 seconds passed, did we actually get here?!");

            return true;
        }
    }
}