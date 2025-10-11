using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.UI
{
    public interface IPopupAlert
    {
        event EventHandler<PopupAlertType> PopupShowed;
        Task ShowError(string message);
        Task ShowSuccess(string message);
        Task ShowInfo(string message);
    }

    public enum PopupAlertType
    {
        None = 0,
        Info = 1,
        Success = 2,
        Error = 3
    }
}
