using BestApp.Abstraction.Main.UI;
using Logging.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.UI
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
