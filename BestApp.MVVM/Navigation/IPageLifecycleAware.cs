using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MVVM.Navigation
{
    public interface IPageLifecycleAware
    {
        void OnAppearing();
        void OnDisappearing();

        void ResumedFromBackground();
        void PausedToBackground();
    }
}
