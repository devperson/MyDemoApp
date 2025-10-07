using BestApp.ViewModels;
using BestApp.ViewModels.Movies;
using IntegrationTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace IntegrationTest
{
    [TestClass]
    public class T1_MovieTest : NavigatableTest
    {
        [TestMethod]
        public async Task T1_1TestMainPageLoad()
        {
            await Navigate(nameof(MoviesPageViewModel));
            await Task.Delay(7000);
            var mainVm = GetNextPage<MoviesPageViewModel>();
            //validate
            Assert.IsTrue(mainVm.MovieItems != null, "Timeout to wait when movie list loaded");
            Assert.IsTrue(mainVm.MovieItems.Count > 0, "No movie items");
            EnsureNoError();
        }

        [TestMethod]
        public async Task T1_2TestAddMoview()
        {
            await Navigate(nameof(MoviesPageViewModel));
            await Task.Delay(1000);
            var mainVm = GetNextPage<MoviesPageViewModel>();
            var oldMovieCount = mainVm.MovieItems.Count;

            await mainVm.AddCommand.ExecuteAsync();
            //navigated to create page
            var createMovieVm = GetNextPage<AddEditMoviePageViewModel>();
            createMovieVm.Name = "integration test movie 1";
            createMovieVm.Overview = "just testing integration test";
            //create movie
            await createMovieVm.CreateCommand.ExecuteAsync();
            EnsureNoError();
            //navigated back to main page
            mainVm = GetNextPage<MoviesPageViewModel>();
            var newCount = mainVm.MovieItems.Count;
            //validate
            Assert.IsTrue(newCount == oldMovieCount + 1, "The old items count should increase to one item");
            EnsureNoError();
        }
    }
}
