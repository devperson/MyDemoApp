using BestApp.ViewModels.Base;
using BestApp.X.Droid.Pages.Base;
using INavigationParameters = BestApp.Abstraction.Main.UI.Navigation.INavigationParameters;

namespace BestApp.X.Droid.Navigation;

public static class NavRegisterar
{
    private static List<NavPageInfo> navPages = new List<NavPageInfo>();

    public static void RegisterPageForNavigation<TPage, TViewModel>(this IContainerRegistry cr)
            where TPage : LifecyclePage
            where TViewModel : PageViewModel
    {
        var type = typeof(TViewModel);
        var name = type.Name;

        var containerRegistry = ContainerLocator.Current;
        containerRegistry.Register<TViewModel>();
        RegisterPageForNavigation(name, () => (TPage)Activator.CreateInstance(typeof(TPage)), () => containerRegistry.Resolve<TViewModel>());
    }

    private static void RegisterPageForNavigation(string vmName, Func<LifecyclePage> createPageFactory, Func<PageViewModel> createVmFactory)
    {
        RegisterPageForNavigation(new NavPageInfo
        {
            VmName = vmName,
            CreatePageFactory = createPageFactory,
            CreateVmFactory = createVmFactory
        });
    }
    private static void RegisterPageForNavigation(NavPageInfo pageInfo)
    {
        if (navPages.All(n => n.VmName != pageInfo.VmName))
            navPages.Add(pageInfo);
    }

    public static LifecyclePage CreatePage(string vmName, INavigationParameters parameters)
    {
        var pageInfo = navPages.FirstOrDefault(p => p.VmName == vmName);
        if (pageInfo != null)
        {
            var page = pageInfo.CreatePageFactory();
            page.ViewModel = pageInfo.CreateVmFactory();
            page.ViewModel.Initialize(parameters);

            return page;
        }
        else
        {
            throw new Exception($"ViewModel '{vmName}' was not registered for page navigation.");
        }
    }
}

public class NavPageInfo
{
    public string VmName { get; set; }
    public Func<LifecyclePage> CreatePageFactory { get; set; }
    public Func<PageViewModel> CreateVmFactory { get; set; }
}

