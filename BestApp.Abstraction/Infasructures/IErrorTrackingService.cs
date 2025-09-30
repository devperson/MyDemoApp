using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures
{
    public interface IErrorTrackingService
    {
        event EventHandler<Exception> OnServiceError;
        void TrackError(Exception ex, byte[] attachment = null, Dictionary<string, string> addionalData = null);
    }
}
