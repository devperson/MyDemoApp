using BestApp.Abstraction.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.InfrasImpl.Base;
using DryIoc;
using Base.Infrastructures.Abstractions.Repository;

namespace UnitTest.InfrasImpl
{
    [TestClass]
    public class SqliteUnitTest : IoCAware
    {
        
        [TestMethod]
        public async Task T1TestMovieTable()
        {
            var movieRepo = Container.Resolve<IRepository<Movie>>();
            var movie = new Movie()
            {
                Name = "test movie from unittest",
                Overview = "good movie",
                PosterUrl = "no url"
            };
            await movieRepo.AddAsync(movie);

            Assert.IsTrue(movie.Id > 0);
        }
    }
}
