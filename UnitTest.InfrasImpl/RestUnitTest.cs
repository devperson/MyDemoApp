using BestApp.Abstraction.General.Infasructures.REST;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UnitTest.InfrasImpl.Base;
using DryIoc;
using System.Threading.Tasks;

namespace UnitTest.InfrasImpl
{
    [TestClass]
    public class RestServiceTest : IoCAware
    {
        [TestMethod]
        public async Task T1TestMovieRest()
        {
            var movieRestService = Container.Resolve<IMovieRestService>();
            var list = await movieRestService.GetMovieRestlist();

            Assert.IsTrue(list.Any());
            Assert.IsTrue(list.FirstOrDefault()?.PosterUrl.Contains("http"));
        }
    }
}
