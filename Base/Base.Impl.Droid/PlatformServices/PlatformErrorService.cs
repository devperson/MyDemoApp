using Base.Abstractions.PlatformServices;

namespace Base.Impl.Droid.PlatformServices
{
    public class PlatformErrorService : IPlatformErrorService
    {
        public bool IsHttpRareError(Exception ex)
        {
            //check for http native errors that happens very rarely due to device or native component bugs
            var val = ex is Java.IO.IOException || ex is Java.IO.EOFException || ex is Java.Net.UnknownHostException || ex is Java.Net.SocketException || ex is Java.Net.ConnectException || ex.ToString().Contains("Code=-1009");//happens rarely in com.android.okhttp of native http handler

            return val;
        }
    }
}
