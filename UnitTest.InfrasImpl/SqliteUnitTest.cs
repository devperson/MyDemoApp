using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.InfrasImpl.Base;
using DryIoc;

namespace UnitTest.InfrasImpl
{
    [TestClass]
    public class SqliteUnitTest : IoCAware
    {
        [TestMethod]
        public async Task T1TestProductTable()
        {
            var productPepo = Container.Resolve<IRepository<Product>>();
            var product = new Product()
            {
                Name = "sql test prodcut",
                Active = true,
                Cost = 6,
                Created = DateTime.Now,
                Quantity = 1,
            };
            await productPepo.Add(product);

            Assert.IsTrue(product.Id > 0);
        }

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
            await movieRepo.Add(movie);

            Assert.IsTrue(movie.Id > 0);
        }
    }
}
