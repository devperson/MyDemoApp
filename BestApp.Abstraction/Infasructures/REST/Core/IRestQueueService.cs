using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures.REST
{
    public interface IRestQueueService
    {
        void Resume();
        void Pause();
        void Release();
    }
}
