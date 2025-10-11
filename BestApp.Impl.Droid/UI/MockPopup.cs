using Base.Abstractions.UI;
using Base.Aspect;

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
