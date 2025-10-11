using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.ViewModel.Base;
using DryIoc;
using BestApp.ViewModels.Movies;
using Base.Abstractions.Diagnostic;

namespace UnitTest.ViewModel
{
    [TestClass]
    public class MainViewModelTest : IoCAware
    {
        [TestMethod]
        public async Task T1_1TestLoadMethod()
        {
            var mainVm = Container.Resolve<MoviesPageViewModel>();
            await mainVm.LoadData();

            Assert.IsTrue(mainVm.MovieItems.Any());
            var loggingService = Container.Resolve<ILoggingService>();
            Assert.IsFalse(loggingService.HasError);
        }

        [TestMethod]
        public async Task T1_2TestNavigateToCreateProduct()
        {
            var mainVm = Container.Resolve<MoviesPageViewModel>();
            await mainVm.AddCommand.ExecuteAsync();

            var loggingService = Container.Resolve<ILoggingService>();
            Assert.IsFalse(loggingService.HasError);
        }

        [TestMethod]
        public async Task T1_3TestPullRefresh()
        {
            var mainVm = Container.Resolve<MoviesPageViewModel>();
            await mainVm.RefreshCommand.ExecuteAsync();

            var loggingService = Container.Resolve<ILoggingService>();
            Assert.IsFalse(loggingService.HasError);
        }
    }
}
