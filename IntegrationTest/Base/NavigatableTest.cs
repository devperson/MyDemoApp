using BestApp.MVVM.Navigation;
using BestApp.ViewModels.Base;
using Common.Abstrtactions;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.Base
{
    public class NavigatableTest : IoCAware
    {
        /// <summary>
        /// This method called every time when test method completes
        /// </summary>
        [TestCleanup]
        public void LogOut()
        {
            LogMessage("***********TEST method ends****************");            
        }

        protected PageViewModel GetCurrentPage()
        {
            var navigation = Container.Resolve<IPageNavigationService>();
            var page = navigation.GetCurrentPageModel() as PageViewModel;
            return page;
        }

        protected void EnsureNoError()
        {
            var log = Container.Resolve<ILoggingService>();
            if (log.HasError)
            {
                var exc = log.LastError;
                log.LastError = null;

                throw new LogLastErrorException(exc.ToString());
            }
        }

        protected Task Navigate(string name)
        {
            var nav = Container.Resolve<IPageNavigationService>();
            return nav.Navigate(name);
        }

        protected void ThrowWrongPageError<CorrectPageT>(PageViewModel wrongPage)
        {
            var correctPageName = typeof(CorrectPageT).Name;
            var wrongPageName = wrongPage.GetType().Name;
            var error = $"App should be navigated to {correctPageName} but navigated to {wrongPageName}.";
            ThrowException(error);
        }

        protected T GetNextPage<T>() where T : PageViewModel
        {
            EnsureNoError();
            var page = GetCurrentPage();
            if (page is T)
            {
                return (T)page;
            }
            else
            {
                ThrowWrongPageError<T>(page);
                return null;
            }
        }

        protected async Task WaitPage<T>()
        {
            LogMessage($"WaitPage(): Start waiting page type {typeof(T)}");
            var stopWaiting = false;
            while (!stopWaiting)
            {
                _ = Task.Run(() =>
                {
                    Task.Delay(20000);
                    LogMessage($"WaitPage(): Time out waititng the page type {typeof(T)}. Stopping the cycle");
                    stopWaiting = true;
                });

                var currentPage = GetCurrentPage();
                if (currentPage is T)
                {
                    LogMessage($"WaitPage(): Got the awaited page type {typeof(T)}. Stopping the cycle");
                    break;
                }
                else
                {
                    LogMessage($"WaitPage(): The current page type {currentPage.GetType()} is not equal {typeof(T)}, continue waiting the page");
                    await Task.Delay(1000);
                }
            }
        }
    }

    public class LogLastErrorException : Exception
    {
        public LogLastErrorException(string error) : base(error)
        {

        }
    }
}
