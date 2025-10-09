using BestApp.Abstraction.Main.AppService;
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
            var result = await movieService.AddAsync("first movie test", "test overview", string.Empty);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Value.Id > 0);
        }

        [TestMethod]
        public async Task T1_3GetMovieListTest()
        {
            var movieService = Container.Resolve<IMoviesService>();
            var result = await movieService.GetListAsync();

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Value.Count > 0);
            Assert.IsTrue(result.Value[0].Id > 0);
            Assert.IsNotNull(result.Value[0].Name);
        }
    }
}
