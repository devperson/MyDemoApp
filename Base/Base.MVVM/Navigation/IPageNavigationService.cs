using Base.MVVM.ViewModels;
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
        IPage GetCurrentPage();        
        PageViewModel GetCurrentPageModel();
        PageViewModel GetRootPageModel();
        List<PageViewModel> GetNavStackModels();
        bool CanNavigateBack { get; }
    }
}
