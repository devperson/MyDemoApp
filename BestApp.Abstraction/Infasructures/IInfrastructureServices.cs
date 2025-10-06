using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures
{
    public interface IInfrastructureServices
    {
        Task Start();
        Task Pause();
        Task Resume();
        Task Stop();
    }
}
