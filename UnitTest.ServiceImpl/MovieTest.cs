using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.ServiceImpl.Base;

namespace UnitTest.ServiceImpl
{
    [TestClass]
    public class MovieTest : IoCAware
    {
        [TestMethod]
        public async Task T1_1AddMovieTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.AddAsync("first product", "test overview", string.Empty);
            
            Assert.IsTrue(result.Success, "IMoviesService.AddAsync() failed in T1_1AddMovieTest()");
            Assert.IsTrue(result.Value.Id > 0, "Movie item id is zero in T1_1AddMovieTest()");            
        }

        [TestMethod]
        public async Task T1_2GetMovieListTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.GetListAsync();

            Assert.IsTrue(result.Success, "IMoviesService.GetListAsync() failed in T1_2GetMovieListTest()");
            Assert.IsTrue(result.Value.Count > 0, "Movie count is zero in T1_2GetMovieListTest()");
            Assert.IsTrue(result.Value[0].Id > 0, "Movie item id is zero in T1_2GetMovieListTest()");
        }

        [TestMethod]
        public async Task T1_3GetMovieTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.GetById(1);
            Assert.IsTrue(result.Success, "IMoviesService.GetById() failed in T1_3GetMovieTest()");
        }

        [TestMethod]
        public async Task T1_4UpdateMovieTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.GetById(1);
            Assert.IsTrue(result.Success, "IMoviesService.GetById() failed in T1_3GetMovieTest()");

            var item = result.Value;            
            item.Name = "updated name";
            item.Overview = "updated overview";
            item.PosterUrl = "updated poster";
            var updateResult = await movieService.UpdateAsync(item);
            Assert.IsTrue(updateResult.Success, "IMoviesService.UpdateAsync() failed in T1_3UpdateMovieTest()");
        }

        [TestMethod]
        public async Task T1_5RemoveMovieTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.GetById(1);
            Assert.IsTrue(result.Success, "IMoviesService.GetById() failed in T1_3UpdateMovieTest()");

            var item = result.Value;
            var removeResult = await movieService.RemoveAsync(item);
            Assert.IsTrue(removeResult.Success, "IMoviesService.RemoveAsync() failed in T1_3RemoveMovieTest()");
        }
    }
}
