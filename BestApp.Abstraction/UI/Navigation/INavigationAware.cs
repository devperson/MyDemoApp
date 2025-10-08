using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = BestApp.Abstraction.Main.UI.Navigation.INavigationParameters;

namespace BestApp.Abstraction.Main.UI.Navigation
{
    public interface INavigationAware
    {
        /// <summary>
        /// Called when the implementer has been navigated away from.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        void OnNavigatedFrom(INavigationParameters parameters);

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        void OnNavigatedTo(INavigationParameters parameters);
    }
}
