using BestApp.Abstraction.Main.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.iOS.Platform
{
    internal class PlatformErrorService : IPlatformErrorService
    {
        public bool IsHttpRareError(Exception ex)
        {
            //check for http native errors that happens very rarely due to device or native component bugs
            return ex.ToString().Contains("Code=-1009") || ex.ToString().Contains("Code=-1005") || ex is HttpRequestException;
        }
    }
}
