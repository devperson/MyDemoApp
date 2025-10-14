using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.UI;

public interface IAlertDialogService
{
    Task DisplayAlert(string title, string message, string cancel = "Close");
    Task<bool> ConfirmAlert(string title, string message, params string[] buttons);
    

    Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    Task<string> DisplayActionSheet(string title, params string[] buttons);    
}
