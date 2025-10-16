using Base.Impl.Texture.iOS.Pages;
using Base.MVVM.Navigation;
using Base.MVVM.ViewModels;
using Microsoft.Maui.ApplicationModel;

namespace Base.Impl.iOS.UI.Navigation;

public class iOSPageNavigationController : UINavigationController, IPageNavigationService
{
    internal iOSLifecyclePage currentPage;

    public iOSPageNavigationController()
    {
        //hide top navigation bar
        this.SetNavigationBarHidden(true, false);
    }

    public bool CanNavigateBack 
    {
        get
        {
            return this.ViewControllers?.Length > 1;
        }
    }

    public async Task Navigate(string url, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true, bool wrapIntoNav = false)
    {
        if (parameters == null)
        {
            parameters = new NavigationParameters();
        }

        var navInfo = UrlNavigationHelper.Parse(url);

        if (navInfo.isPush)
        {
            await this.OnPushAsync(url, parameters, animated);
        }
        else if (navInfo.isPop)
        {
            await this.OnPopAsync(parameters);
        }
        else if (navInfo.isMultiPop)
        {
            await this.OnMultiPopAsync(url, parameters, animated);
        }
        else if (navInfo.isMultiPopAndPush)
        {
            await this.OnMultiPopAndPush(url, parameters, animated);
        }
        else if (navInfo.isPushAsRoot)
        {
            await this.OnPushRootAsync(url, parameters, animated);
        }
        else if (navInfo.isMultiPushAsRoot)
        {
            await this.OnMultiPushRootAsync(url, parameters, animated);
        }
        else
        {
            throw new NotImplementedException("Navigation case is not implemented.");
        }

        var controllers = this.ViewControllers;
    }
   
    private async Task OnPushAsync(string vmName, INavigationParameters parameters, bool animated)
    {
        var currentToHide = currentPage;
        currentPage = NavRegistrar.CreatePage(vmName, parameters) as iOSLifecyclePage;

        currentToHide.ViewModel.OnNavigatedFrom(null);
        currentPage.ViewModel.OnNavigatedTo(parameters);

        this.PushViewController(currentPage, animated);

        //wait for when page gets appeared
        var showed = await PageAppearedAsync(currentPage, animated);
    }

    private async Task OnPopAsync(INavigationParameters parameters)
    { 
        var animated = currentPage.pushNavAnimated;
        var oldPage = currentPage;       
        var index = this.ViewControllers.Length - 2;
        var newPage = currentPage = this.ViewControllers[index] as iOSLifecyclePage;
        this.PopViewController(animated);

        oldPage.ViewModel.OnNavigatedFrom(null);
        newPage.ViewModel.OnNavigatedTo(parameters);

        //wait for when page gets disappeared
        var disapeared = await this.PageDisappearedAsync(oldPage, animated);
        oldPage.Destroy();
    }

    private async Task OnMultiPopAsync(string url, INavigationParameters parameters, bool animated)
    {
        var arrayToRemove = new List<iOSLifecyclePage>();
        var vcs = this.ViewControllers.ToList();
        var splitedCount = url.Split('/').Length - 1;
        for (int i = 0; i < splitedCount; i++)
        {
            //remove top page one by one             
            var page = this.ViewControllers.Last() as iOSLifecyclePage;            
            vcs.Remove(page);

            arrayToRemove.Add(page);
        }

        currentPage = vcs.FirstOrDefault() as iOSLifecyclePage;
        this.PopToViewController(currentPage, animated);

        //call viewmodel lifecycle methods
        currentPage.ViewModel.OnNavigatedTo(parameters);

        //wait for when page gets appeared
        var showed = await this.PageAppearedAsync(currentPage, animated);

        foreach (var removedVc in arrayToRemove)
        {
            removedVc.Destroy();
        }
    }

    private async Task OnMultiPopAndPush(string url, INavigationParameters parameters, bool animated)
    {
        var arrayToRemove = new List<iOSLifecyclePage>();
        var splitedCount = url.Split('/').Length - 1;
        for (int i = 0; i < splitedCount; i++)
        {
            var page = this.ViewControllers.Last() as iOSLifecyclePage;
            arrayToRemove.Add(page);
        }

        var vmName = url.Replace("../", string.Empty);
        currentPage = NavRegistrar.CreatePage(vmName, parameters) as iOSLifecyclePage;
        currentPage.ViewModel.OnNavigatedTo(parameters);

        //now when we get page navigation completed we can do remove back pages
        var vcs = this.ViewControllers.ToList();
        vcs.RemoveAll(v => arrayToRemove.Contains(v));
        vcs.Add(currentPage);
        this.SetViewControllers(vcs.ToArray(), animated);


        //wait for when page gets appeared
        var showed = await this.PageAppearedAsync(currentPage, animated);

        foreach (var removedVc in arrayToRemove)
        {
            removedVc.Destroy();
        }
    }

    private async Task OnPushRootAsync(string url, INavigationParameters parameters, bool animated)
    {
        var vmName = url.Replace("/", string.Empty).Replace("NavigationPage", "");
        currentPage = NavRegistrar.CreatePage(vmName, parameters) as iOSLifecyclePage;
        currentPage.ViewModel.OnNavigatedTo(parameters);

        //now when we get page navigation completed we can do remove back pages
        var vcs = this.ViewControllers.ToList();
        var vcsToRemove = vcs.Select(v=>v as iOSLifecyclePage).ToList();
        vcs.Clear();
        vcs.Add(currentPage);
        this.SetViewControllers(vcs.ToArray(), animated);

        //wait for when page gets appeared
        var showed = await this.PageAppearedAsync(currentPage, animated);

        foreach (var removedVc in vcsToRemove)
        {
            removedVc.Destroy();
        }
    }

    private async Task OnMultiPushRootAsync(string url, INavigationParameters parameters, bool animated)
    {
        var cleanUrl = url.Replace("/NavigationPage", string.Empty);
        var vmPages = cleanUrl.Split("/").Where(s => !string.IsNullOrEmpty(s)).ToList();
        var newPages = new List<iOSLifecyclePage>();
        var oldPages = this.ViewControllers.Cast<iOSLifecyclePage>().ToList();

        foreach (var vmName in vmPages)
        {
            var page = NavRegistrar.CreatePage(vmName, parameters) as iOSLifecyclePage;            
            page.pushNavAnimated = animated;
            newPages.Add(page);  
        }

        this.SetViewControllers(newPages.ToArray(), animated);

        currentPage = newPages.Last();
        currentPage.ViewModel.OnNavigatedTo(parameters);

        //wait for when page gets appeared
        var showed = await this.PageAppearedAsync(currentPage, animated);

        foreach (var oldPage in oldPages)
        {
            oldPage.Destroy();
        }
    }

    private async Task OnPopToRootAsync(INavigationParameters parameters)
    {
        if (this.ViewControllers.Length <= 1)
        {
            return;
        }
        else if (this.ViewControllers.Length == 2)
        {
            await this.OnPopAsync(parameters);
        }
        else
        {
            currentPage = this.ViewControllers[0] as iOSLifecyclePage;
            var vcsToRemove = this.ViewControllers.Skip(1).Select(v => v as iOSLifecyclePage).ToArray();
            this.PopToRootViewController(true);
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //wait for when page gets appeared
            var showed = await this.PageAppearedAsync(currentPage, true);

            foreach (var removedVc in vcsToRemove)
            {
                removedVc.Destroy();
            }
        }
    }


    private Task<bool> PageAppearedAsync(iOSLifecyclePage page, bool animated)
    {        
        if (animated == false)
        {
            return Task.FromResult(true);
        }

        var tcs = new TaskCompletionSource<bool>();
        EventHandler appearing = null;
        appearing = (s, e) =>
        {
            page.Appeared -= appearing;            

            MainThread.BeginInvokeOnMainThread(() => 
            { 
                tcs.SetResult(true); 
            });
        };

        page.Appeared -= appearing;        
        page.Appeared += appearing;        

        return tcs.Task;
    }

    private Task<bool> PageDisappearedAsync(iOSLifecyclePage page, bool animated)
    {
        if (animated == false)
        {
            return Task.FromResult(true);
        }

        var tcs = new TaskCompletionSource<bool>();
        EventHandler disappearing = null;        
        disappearing = (s, e) =>
        {         
            page.Disappeared -= disappearing;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                tcs.SetResult(false);
            });
        };

        page.Disappeared -= disappearing;
        page.Disappeared += disappearing;

        return tcs.Task;
    }

    public List<PageViewModel> GetNavStackModels()
    {
        var viewModels = this.ViewControllers.OfType<iOSLifecyclePage>().Select(x => x.ViewModel).ToList();
        return viewModels;
    }

    public PageViewModel GetRootPageModel()
    {
        var rootPage = this.ViewControllers.OfType<iOSLifecyclePage>().FirstOrDefault();
        return rootPage?.ViewModel;
    }

    public PageViewModel GetCurrentPageModel()
    {
        var page = this.ViewControllers?.LastOrDefault() as iOSLifecyclePage;
        return page?.ViewModel;
    }

    public IPage GetCurrentPage()
    {
        var page = this.ViewControllers?.LastOrDefault() as iOSLifecyclePage;
        return page;
    }

    public async Task NavigateToRoot(INavigationParameters parameters)
    {
        await this.OnPopToRootAsync(parameters);
    }

}
