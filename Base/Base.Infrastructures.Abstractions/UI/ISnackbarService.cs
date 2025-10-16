using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.UI;
public interface ISnackbarService
{
    event EventHandler<SeverityType> PopupShowed;
    void ShowError(string message);
    void ShowInfo(string message);    
    void Show(string message, SeverityType severityType, int duration = 3000);
}

public enum SeverityType
{
    Info,
    Success,
    Warning,
    Error
}

