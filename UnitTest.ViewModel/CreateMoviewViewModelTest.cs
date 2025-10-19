using Base.Abstractions.Diagnostic;
using Base.Abstractions.UI;
using Base.MVVM.Navigation;
using BestApp.ViewModels.Movies;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var popupAlert = Container.Resolve<ISnackbarService>();
            var createVm = Container.Resolve<AddEditMoviePageViewModel>();
            createVm.Initialize(new NavigationParameters());
            int errorCount = 0;
            popupAlert.PopupShowed += (s, popupType) =>
            {
                if(popupType == SeverityType.Error)
                    errorCount++;
            };
            //check Name validation
            await createVm.SaveCommand.ExecuteAsync();
            await Task.Delay(200);//small delay to make sure that errorCount updated
            Assert.IsTrue(errorCount == 1, $"failed: name validation, the errorCount: {errorCount}");

            //check Overview validation
            createVm.Model.Name = "Test movie1";            
            await createVm.SaveCommand.ExecuteAsync();
            await Task.Delay(200);//small delay to make sure that errorCount updated
            Assert.IsTrue(errorCount == 2, $"failed: Overview validation, the errorCount: {errorCount}");            

            //Create product
            createVm.Model.Overview = "test overview1";
            createVm.Model.PosterUrl = string.Empty;
            //note that name, overview should be the same that the moq expects (name:"Test movie1", Overview:test overview1) (see IoC registration)
            await createVm.SaveCommand.ExecuteAsync();
            await Task.Delay(200);//small delay to make sure that errorCount updated
            Assert.IsTrue(errorCount == 2, "validation error");
            Assert.IsFalse(loggingService.HasError, $"There is another error beside validation error, the errorCount: {errorCount}");
        }
    }
}
