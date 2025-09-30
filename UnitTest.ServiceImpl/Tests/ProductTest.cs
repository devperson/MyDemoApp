using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.AppService.Dto;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.ServiceImpl.Base;

namespace UnitTest.ServiceImpl.Tests
{
    [TestClass]
    public class ProductTest : IoCAware
    {
        [TestMethod]
        public async Task T1_1AddProductTest()
        {
            var productService = Container.Resolve<IProductService>();
            var result = await productService.Add("first product", 1, 5);
            
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Value.Id > 0);            
        }

        [TestMethod]
        public async Task T1_2GetProductTest()
        {
            var productService = Container.Resolve<IProductService>();
            var result = await productService.Get(1);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Value.Id > 0);
        }

        [TestMethod]
        public async Task T1_3GetProductListTest()
        {
            var productService = Container.Resolve<IProductService>();
            var result = await productService.GetSome(count: 10, skip: 0);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Value.Count > 0);
            Assert.IsTrue(result.Value[0].Id > 0);
            Assert.IsNotNull(result.Value[0].Name);
        }
    }
}
