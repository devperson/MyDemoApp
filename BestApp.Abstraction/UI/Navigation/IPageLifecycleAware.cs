using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.UI.Navigation
{
    public interface IPageLifecycleAware
    {
        void OnAppearing();
        void OnDisappearing();

        void ResumedFromBackground();
        void PausedToBackground();
    }
}
