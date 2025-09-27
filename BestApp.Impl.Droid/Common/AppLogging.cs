using Android.Util;
using Common.Abstrtactions;

namespace BestApp.Impl.Droid
{
    public class AppLogging : ILoggingService
    {
        public void Log(string message)
        {
            Android.Util.Log.Info("AppLogging", message);
        }

        public void LogWarning(string message)
        {
            Android.Util.Log.Warn("AppLogging", message);
        }

        public void TrackError(Exception ex)
        {
            Android.Util.Log.Error("AppLogging", ex.ToString());
        }
    }
}
