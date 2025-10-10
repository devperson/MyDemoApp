using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MVVM.Navigation
{
    public interface IPageNavigationService
    {
        Task Navigate(string name, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true, bool wrapIntoNav = false);
        Task NavigateToRoot(INavigationParameters parameters);
        Task CloseModal(INavigationParameters parameters = null);
        bool HasPageInNavigation(string page);
        object GetCurrentPageModel();
        object GetRootPageModel();
        List<object> GetNavStackModels();
        bool CanNavigateBack { get; }
    }
}
