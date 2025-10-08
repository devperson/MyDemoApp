using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.Platform;
using Common.Abstrtactions;
using System.Text;

namespace BestApp.Impl.Cross.Common
{
    public class AppLoggingService : ILoggingService
    {
        private long rowNumber = 0;
        private const string ENTER_TAG = "➡Enter";
        private const string EXIT_TAG = "🏃Exit";
        private const string INDICATOR_TAG = "⏱Indicator_";
        private const string HandledError = "💥Handled Exception";
        private const string UnhandledError = "💥Crash Unhandled";

        private readonly IErrorTrackingService errorTrackingService;
        private readonly IFileLoger fileLoger;
        private readonly Lazy<IPreferencesService> preferences;
        private int appLaunchCount;
        public event EventHandler<Exception> ErrorRegistered;
        public bool HasError => LastError != null;
        public Exception LastError { get; set; }

        public AppLoggingService(IErrorTrackingService errorTrackingService, IFileLoger fileLoger, Lazy<IPreferencesService> preferences)
        {
            errorTrackingService.OnServiceError += ErrorTrackingService_OnError;

            this.errorTrackingService = errorTrackingService;
            this.fileLoger = fileLoger;
            this.preferences = preferences;
            this.fileLoger.Init();
            appLaunchCount = this.GetLaunchCount();
        }

        private void ErrorTrackingService_OnError(object sender, Exception e)
        {
            LogError(e, "Error happens in IErrorTrackingService", handled: true);
        }

        public void TrackError(Exception ex, Dictionary<string, string> data = null)
        {
            TrackInternal(ex, true, data);
        }

        public void LogUnhandledError(Exception ex)
        {
            TrackInternal(ex, false);
        }


        private void TrackInternal(Exception ex, bool handled, Dictionary<string, string> data = null)
        {
            LastError = ex;
            ErrorRegistered?.Invoke(this, LastError);

            LogError(ex, string.Empty, handled: handled);
            //send to Crash service only if it is not Debug and Handled in try/catch
            //unhandled crashes will be automatically send in next app startup
            if (handled == true)
            {
                var attachments = GetLastSessionLogBytes();
                errorTrackingService.TrackError(ex, attachments);
            }
        }

        public void Log(string message)
        {
            rowNumber++;
            var tag = GetLogAppTag(appLaunchCount, rowNumber);
            //add app_tag for logging
            message = $"{tag} INFO:{message}";

            //LOG TO FILE and Memory
            fileLoger.Info(message);

            //for debug
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            rowNumber++;
            var tag = GetLogAppTag(appLaunchCount, rowNumber);
            //add app_tag for logging
            message = $"{tag} WARNING:{message}";

            //LOG TO FILE and Memory
            fileLoger.Warn(message);
            //for debug
            Console.WriteLine(message);
        }

        public void LogError(Exception ex, string message, bool handled = true)
        {
            rowNumber++;
            var tag = GetLogAppTag(appLaunchCount, rowNumber);

            //add app_tag for logging
            var formattedMessage = "";
            if (!string.IsNullOrEmpty(message))
            {
                if (handled)
                {
                    formattedMessage = $"{tag} ERROR: {message}, {HandledError}:";
                }
                else
                {
                    formattedMessage = $"{tag} ERROR: {message}, {UnhandledError}:";
                }
            }
            else
            {
                if (handled)
                {
                    formattedMessage = $"{tag} ERROR: {HandledError}:";
                }
                else
                {
                    formattedMessage = $"{tag} ERROR: {UnhandledError}:";
                }
            }

            //LOG TO FILE and Memory
            fileLoger.Warn(formattedMessage, ex);

            //for debug
            Console.WriteLine($"{formattedMessage}: {ex}");
        }

        public static string GetLogAppTag(int appLaunchCount, long rowNumber)
        {
            var localDate = DateTime.Now.ToLocalTime();
            var logtag = $"S({appLaunchCount})_R({rowNumber})_D({localDate.ToString("HH:mm:ss.fff")})";

            return logtag;
        }

        public MemoryStream GetCompressedLogFileStream(bool getOnlyLastSession = false)
        {
            //get all logs as single .zip file
            var stream = this.fileLoger.GetCompressedLogsSync(getOnlyLastSession);

            if (stream != null)
                return (MemoryStream)stream;
            else
                return null;
        }

        public async Task<string> GetSomeLogTextAsync()
        {
            var stringBuild = new StringBuilder();
            var lines = await this.fileLoger.GetLogListAsync();
            stringBuild.AppendJoin(Environment.NewLine, lines);
            var text = stringBuild.ToString();

            return text;
        }


        public void LogMethodStarted(string methodName)
        {
            Log($"{ENTER_TAG} {methodName}");
        }

        public void LogMethodFinished(string methodName)
        {
            Log($"{EXIT_TAG} {methodName}");
        }

        public void Header(string headerMessage)
        {
            //LOG TO FILE and Memory
            fileLoger.Info(headerMessage);

            //for debug
            Console.WriteLine(headerMessage);
        }

        public byte[] GetLastSessionLogBytes()
        {
            try
            {
                var compressedLogs = this.GetCompressedLogFileStream(true);
                return compressedLogs.ToArray();
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get App Log");
                return null;
            }
        }

        private int GetLaunchCount()
        {   
            var launchCount = preferences.Value.Get("AppLaunchCount", 0);
            launchCount += 1;
            preferences.Value.Set("AppLaunchCount", launchCount);

            return launchCount;
        }

        public void LogIndicator(string name, string message)
        {
            //add app_tag for logging
            message = $"********************************{INDICATOR_TAG}{name}*************************************";

            //LOG TO FILE and Memory
            fileLoger.Info(message);

            //for debug
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes the log message without lock/semaphor
        /// </summary>
        public void ForceToLog(Exception ex)
        {
            //Currently when unhandled crash occurs the lock/semaphor stops working thus we need to disable lock to write the error to the log
            //fileLoger.DisableSemaphor();
            LogError(ex, null, handled: false);
        }

        public string GetLogsFolder()
        {
            return fileLoger.GetLogsFolder();
        }

        //    private void Copy_iOS_ExtenstionLogs()
        //    {
        //#if __IOS__
        //            try
        //            {
        //                //Copy iOS Extenstion logs to MetroLogs folder
        //                var sharedPath = Foundation.NSFileManager.DefaultManager.GetContainerUrl(Constants.SHARE_GROUP_INDENTIFIER).Path;
        //                var iosExtLogsDir = Path.Combine(sharedPath, "Logs");
        //                if (Directory.Exists(iosExtLogsDir))
        //                {
        //                    var metroLogDir = this.GetLogsFolder();
        //                    foreach (var file in Directory.EnumerateFiles(iosExtLogsDir))
        //                    {
        //                        File.Copy(file, metroLogDir, true);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LogWarning("Unable to copy iOS extenstion logs to MetroLogs directory. " + ex);
        //            }
        //#endif
        //    }
    }
}
