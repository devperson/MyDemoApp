using Base.Abstractions.UI;
using Base.Impl.Texture.iOS.Pages;
using Base.MVVM.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Texture.iOS.UI;
public class iOSSnackbarService : ISnackbarService
{
    private readonly IPageNavigationService pageNavigationService;

    public event EventHandler<SeverityType> PopupShowed;

    public iOSSnackbarService(IPageNavigationService pageNavigationService)
    {
        this.pageNavigationService = pageNavigationService;
    }
    public void ShowError(string message)
    {        
        this.Show(message, SeverityType.Error);
    }

    public void ShowInfo(string message)
    {
        this.Show(message, SeverityType.Info);
    }

    public void Show(string message, SeverityType severityType, int duration = 3000)
    {
        PopupShowed?.Invoke(this, severityType);

        var page = pageNavigationService.GetCurrentPage() as iOSLifecyclePage;

        if (page != null)
        {
            page.snackbarNode.SetText(message, severityType);
            page.snackbarNode.Show();
        }
    }
}
