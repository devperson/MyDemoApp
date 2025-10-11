using Base.Abstractions.Diagnostic;
using System.Diagnostics;

namespace UnitTest.Impl
{
    internal class MockAppLogging : ILoggingService
    {
        private const string ENTER_TAG = "➡Enter";
        private const string EXIT_TAG = "🏃Exit";
        private const string INDICATOR_TAG = "⏱Indicator_";

        public event EventHandler<Exception> ErrorRegistered;

        public Exception LastError { get; set; }

        public bool HasError
        {
            get
            {
                if (LastError == null)
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public MemoryStream GetCompressedLogFileStream()
        {
            return new MemoryStream();
        }

        public Task<string> GetSomeLogTextAsync()
        {
            return Task.Run(() => "test");
        }

        public void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffffff")}_{message}");
        }

        public void LogMethodStarted(string methodName)
        {
            Log($"{ENTER_TAG} {methodName}");
        }

        public void LogMethodFinished(string methodName)
        {
            Log($"{EXIT_TAG} {methodName}");
        }

        public void TrackError(Exception ex, Dictionary<string, string> data = null)
        {
            LastError = ex;
            ErrorRegistered?.Invoke(this, LastError);
        }


        public void LogUnhandledError(Exception ex)
        {
            Log(ex.ToString());
        }

        public void LogWarning(string message)
        {
            Log($"WARNING: {message}");
        }

        public void LogError(Exception ex, string message, bool handled = true)
        {
            Log($"ERROR: {message},💥Handled Exception: {ex}");
        }

        public void Header(string headerMessage)
        {

        }

        public void LogIndicator(string name, string message)
        {
            //add app_tag for logging
            message = $"********************************{INDICATOR_TAG}{name}*************************************";

            //LOG TO FILE and Memory
            Debug.WriteLine($"{message}");

            //for debug
            Debug.WriteLine(message);
        }

        public string GetLogsFolder()
        {
            return string.Empty;
        }
       

        public MemoryStream GetCompressedLogFileStream(bool getOnlyLastSession = false)
        {
            throw new NotImplementedException();
        }

        public byte[] GetLastSessionLogBytes()
        {
            throw new NotImplementedException();
        }
    }
}
