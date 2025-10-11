using Base.Abstractions.Platform;
using Base.Abstractions.PlatformServices;
using System;
using System.Net.Http;

namespace Base.Impl.iOS.Platform
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
