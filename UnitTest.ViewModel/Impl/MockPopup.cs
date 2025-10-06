using BestApp.Abstraction.General.UI;
using Logging.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ViewModel.Impl
{
    [LogMethods]
    internal class MockPopup : IPopupAlert
    {
        public event EventHandler<PopupAlertType> PopupShowed;

        public Task ShowError(string message)
        {
            PopupShowed?.Invoke(this, PopupAlertType.Error);
            return Task.CompletedTask;
        }

        public Task ShowInfo(string message)
        {
            PopupShowed?.Invoke(this, PopupAlertType.Info);
            return Task.CompletedTask;
        }

        public Task ShowSuccess(string message)
        {
            PopupShowed?.Invoke(this, PopupAlertType.Success);
            return Task.CompletedTask;
        }
    }
}
