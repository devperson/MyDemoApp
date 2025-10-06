using BestApp.ViewModels.Base;
using BestApp.ViewModels.Helper.Commands;
using BestApp.ViewModels.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Login;

public class LoginPageViewModel : PageViewModel
{
    public LoginPageViewModel(InjectedServices services) : base(services)
    {
        this.SubmitCommand = new AsyncCommand(OnSubmitCommand);
    }

    public AsyncCommand SubmitCommand { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    private async Task OnSubmitCommand(object arg)
    {
        await Services.NavigationService.Navigate($"/{nameof(MoviesPageViewModel)}");
    }
}
