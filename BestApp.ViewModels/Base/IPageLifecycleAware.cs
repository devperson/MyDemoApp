using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Base
{
    public interface IPageLifecycleAware
    {
        void OnAppearing();
        void OnDisappearing();

        void ResumedFromBackground();
        void PausedToBackground();
    }
}
