using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstrtactions
{
    public interface ILoggingService
    {
        event EventHandler<Exception> ErrorRegistered;
        Exception LastError { get; set; }
        bool HasError { get; }

        void Log(string message);
        void LogWarning(string message);        
        void Header(string headerMessage);
        void LogMethodStarted(string methodName);
        void LogMethodFinished(string methodName);
        void LogIndicator(string name, string message);
        void LogError(Exception ex, string message, bool handled = true);
        void TrackError(Exception ex, Dictionary<string, string> data = null);
        void LogUnhandledError(Exception ex);

        MemoryStream GetCompressedLogFileStream(bool getOnlyLastSession = false);
        Task<string> GetSomeLogTextAsync();
        string GetLogsFolder();
        byte[] GetLastSessionLogBytes();
    }

    public interface IFileLoger
    {
        void Init();
        void Info(string message);
        void Warn(string message);
        void Warn(string message, Exception ex);
        Stream GetCompressedLogsSync(bool getOnlyLastSession);
        Task<List<string>> GetLogListAsync();
        string GetLogsFolder();
    }

}
