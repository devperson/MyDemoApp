using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = BestApp.MVVM.Navigation.INavigationParameters;

namespace BestApp.MVVM.Navigation
{
    public interface IInitialize
    {
        void Initialize(INavigationParameters parameters);
    }
}
