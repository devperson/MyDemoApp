using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = BestApp.Abstraction.General.Platform.INavigationParameters;

namespace BestApp.ViewModels.Base
{
    public interface IInitialize
    {
        void Initialize(INavigationParameters parameters);
    }
}
