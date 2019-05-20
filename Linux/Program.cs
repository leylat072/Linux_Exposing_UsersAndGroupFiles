using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Linux.Classes;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Linux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            while (true)
            {
                MainAsync().GetAwaiter().GetResult();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        private static PhysicalFileProvider _fileProvider =
  new PhysicalFileProvider("C:\\Users\\abia1\\source\\repos\\Linux\\Linux\\files");
        private static async Task MainAsync()
        {
            IChangeToken token = _fileProvider.Watch("passwd.txt");
            var tcs = new TaskCompletionSource<object>();

            token.RegisterChangeCallback(state =>
                ((TaskCompletionSource<object>)state).TrySetResult(null), tcs);

            await tcs.Task.ConfigureAwait(false);


            Console.WriteLine("quotes.txt changed");
        }
    }
}
