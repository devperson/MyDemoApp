using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstrtactions
{
    public interface ILoggingService
    {
        void Log(string message);
        void LogWarning(string message);
        void TrackError(Exception message);
    }
}
