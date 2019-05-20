using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Linux.Classes
{
    
    public static class watcher
    {
        public static bool passwdFileIsChanged = false;
        public static void  MainAsync()
        {
            //IChangeToken token = _fileProvider.Watch("passwd.txt");
            //var tcs = new TaskCompletionSource<object>();

            //token.RegisterChangeCallback(state =>
            //    ((TaskCompletionSource<object>)state).TrySetResult(null), tcs);

            //await tcs.Task.ConfigureAwait(false);

            // if(token.HasChanged)
            passwdFileIsChanged = true;
            Console.WriteLine("quotes.txt changed");
        }
    }
}
