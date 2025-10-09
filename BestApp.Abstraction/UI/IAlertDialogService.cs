using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.UI;

public interface IAlertDialogService
{
    Task<bool> ConfirmAlert(string title, string message, params string[] buttons);
    Task DisplayAlert(string title, string message);

    Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    Task<string> DisplayActionSheet(string title, params string[] buttons);    
}
