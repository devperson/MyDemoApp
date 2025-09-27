using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.UI
{
    public interface IPopupAlert
    {
        Task ShowError(string message);
        Task ShowSuccess(string message);
        Task ShowInfo(string message);
    }
}
