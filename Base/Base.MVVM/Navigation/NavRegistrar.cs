using Base.MVVM.ViewModels;
using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MVVM.Navigation
{
    public static class NavRegistrar
    {
        private static List<NavPageInfo> navPages = new List<NavPageInfo>();

        public static void RegisterPageForNavigation<TPage, TViewModel>(this IContainer cr)
            where TPage : IPage
            where TViewModel : PageViewModel
        {            
            cr.Register<TViewModel>(Reuse.Transient);
            RegisterPageForNavigation(typeof(TViewModel).Name, () => (TPage)Activator.CreateInstance(typeof(TPage)), () => cr.Resolve<TViewModel>());
        }

        private static void RegisterPageForNavigation(string vmName, Func<IPage> createPageFactory, Func<PageViewModel> createVmFactory)
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

        public static IPage CreatePage(string vmName, INavigationParameters parameters)
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

    internal class NavPageInfo
    {
        public string VmName { get; set; }
        public Func<IPage> CreatePageFactory { get; set; }
        public Func<PageViewModel> CreateVmFactory { get; set; }
    }
}
