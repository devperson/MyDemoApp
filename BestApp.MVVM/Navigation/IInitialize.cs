using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = Base.MVVM.Navigation.INavigationParameters;

namespace Base.MVVM.Navigation
{
    public interface IInitialize
    {
        void Initialize(INavigationParameters parameters);
    }
}
