using BestApp.Abstraction.Main.UI.Navigation;
using BestApp.ViewModels.Base;
using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigationParameters = BestApp.Abstraction.Main.UI.Navigation.INavigationParameters;
using NavigationParameters = BestApp.ViewModels.NavigationParameters;

namespace IntegrationTest.Impl
{
    internal class PageNavigationService : IPageNavigationService
    {
        public PageNavigationService(IContainer container)
        {
            this.container = container;
        }
        public static Dictionary<string, Type> RegisteredPageList = new Dictionary<string, Type>();

        public bool CanNavigateBack => throw new NotImplementedException();
        public Stack<PageViewModel> ViewModelStackList = new Stack<PageViewModel>();
        private readonly IContainer container;

        public Task CloseModal(INavigationParameters parameters = null)
        {
            return Task.CompletedTask;
        }

        public object GetCurrentPageModel()
        {
            if (ViewModelStackList.Count > 0)
            {
                var current = ViewModelStackList.Peek();
                return current;
            }
            else return null;
        }
        public List<object> GetNavStackModels()
        {
            var listViewModels = ViewModelStackList.ToList();
            var listObj = listViewModels.Select(d => (object)d).ToList();
            return listObj;
        }

        public bool HasPageInNavigation(string page)
        {
            if(ViewModelStackList.Any(s=>s.GetType().Name == page))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task Navigate(string name, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true, bool wrapIntoNav = false)
        {
            //call the disappering method for current page
            if (ViewModelStackList.Count > 0)
            {
                var previousPage = GetCurrentPageModel() as PageViewModel;
                previousPage.OnDisappearing();
            }

            string newPageName = name;
            if (name.Contains("../"))//pop previews page
            {
                //remove previous oages
                var splitedCount = name.Split('/').Length - 1;
                for (int i = 1; i <= splitedCount; i++)
                {
                    var previousPage = GetCurrentPageModel() as PageViewModel;
                    ViewModelStackList.Pop();
                    previousPage.Destroy();
                }

                newPageName = name.Replace("../", string.Empty);
            }
            else if (name.Contains("/"))//clear all previous
            {
                //clear all navigation stack
                var list = ViewModelStackList.ToList();
                ViewModelStackList.Clear();
                foreach (var removedPage in list)
                {
                    removedPage.Destroy();
                }

                newPageName = name.Replace("/", string.Empty);
            }

            if (string.IsNullOrEmpty(newPageName))
            {
                if (name.Contains("../") && parameters != null && parameters.Count() > 0)
                {
                    //navigating back with params
                    var page = GetCurrentPageModel() as PageViewModel;
                    page.OnNavigatedTo(parameters);
                }
                return Task.CompletedTask;
            }

            if (!RegisteredPageList.ContainsKey(newPageName))
                throw new Exception($"The {newPageName} type is not registered in the IOC container");

            var pageType = RegisteredPageList[newPageName];
            var newPage = container.Resolve(pageType);//, serviceKey: newPageName);
            //init new viewmodel
            var viewModel = newPage as PageViewModel;
            if (parameters == null)
                parameters = new NavigationParameters();
            viewModel.Initialize(parameters);
            viewModel.OnNavigatedTo(parameters);
            viewModel.OnAppearing();
            viewModel.OnAppeared();

            //call OnNavigatedFrom for current page
            if (ViewModelStackList.Count > 0)
            {
                var current = ViewModelStackList.Peek();
                current.OnNavigatedFrom(null);
            }
            //add new page
            ViewModelStackList.Push(viewModel);

            return Task.CompletedTask;
        }

        public Task NavigateToRoot()
        {
            if (ViewModelStackList.Count <= 1)
            {
                //no need to do anything
            }
            else if (ViewModelStackList.Count == 2)
            {
                var current = ViewModelStackList.Peek();
                ViewModelStackList.Pop();
                current.Destroy();
            }
            else
            {
                while (ViewModelStackList.Count > 1)
                {
                    var current = ViewModelStackList.Peek();
                    ViewModelStackList.Pop();
                    current.Destroy();
                }
            }

            return Task.CompletedTask;
        }

        public object GetRootPageModel()
        {
            throw new NotImplementedException();
        }
    }
}
