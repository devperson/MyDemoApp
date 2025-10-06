using BestApp.Abstraction.General.UI;
using BestApp.ViewModels;
using Common.Abstrtactions;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.ViewModel.Base;

namespace UnitTest.ViewModel
{
    [TestClass]
    public class CreateMoviewViewModelTest : IoCAware
    {
        [TestMethod]
        public async Task T2_1TestCreateProduct()
        {
            var loggingService = Container.Resolve<ILoggingService>();
            var popupAlert = Container.Resolve<IPopupAlert>();
            var createVm = Container.Resolve<CreateMovieViewModel>();            
            int errorCount = 0;
            popupAlert.PopupShowed += (s, popupType) =>
            {
                if(popupType == PopupAlertType.Error)
                    errorCount++;
            };
            //check Name validation
            await createVm.CreateCommand.ExecuteAsync();
            Assert.IsTrue(errorCount == 1, "failed: name validation");

            //check Overview validation
            createVm.Name = "Test movie1";            
            await createVm.CreateCommand.ExecuteAsync();
            Assert.IsTrue(errorCount == 2, "failed: quantity validation");            

            //Create product
            createVm.Overview = "test overview1";
            createVm.PosterImage = string.Empty;
            //note that name, overview should be the same that the moq expects (name:"Test movie1", Overview:test overview1) (see IoC registration)
            await createVm.CreateCommand.ExecuteAsync();
            Assert.IsTrue(errorCount == 2, "validation error");
            Assert.IsFalse(loggingService.HasError, "There is another error beside validation error");
        }
    }
}
