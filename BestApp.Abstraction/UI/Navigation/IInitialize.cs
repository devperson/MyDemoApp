using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = BestApp.Abstraction.General.UI.Navigation.INavigationParameters;

namespace BestApp.Abstraction.General.UI.Navigation
{
    public interface IInitialize
    {
        void Initialize(INavigationParameters parameters);
    }
}
