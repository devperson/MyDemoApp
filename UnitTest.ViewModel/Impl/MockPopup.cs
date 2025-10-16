using Base.Abstractions.UI;
using Base.Aspect;

namespace UnitTest.ViewModel.Impl
{
    [LogMethods]
    internal class MockPopup : ISnackbarService
    {
        public event EventHandler<SeverityType> PopupShowed;

        public void Show(string message, SeverityType severityType, int duration = 3000)
        {
            PopupShowed?.Invoke(this, SeverityType.Info);            
        }

        public void ShowError(string message)
        {
            PopupShowed?.Invoke(this, SeverityType.Error);
        }

        public void ShowInfo(string message)
        {
            PopupShowed?.Invoke(this, SeverityType.Info);
        }

        public void ShowSuccess(string message)
        {
            PopupShowed?.Invoke(this, SeverityType.Success);
        }        
    }
}
