using BestApp.Abstraction.Main.PlatformServices;
using BestApp.Abstraction.Main.UI.Navigation;
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
    private readonly Lazy<IPreferencesService> preferencesService;

    public LoginPageViewModel(InjectedServices services, Lazy<IPreferencesService> preferencesService) : base(services)
    {
        this.SubmitCommand = new AsyncCommand(OnSubmitCommand);
        this.preferencesService = preferencesService;
    }

    public const string IsLoggedIn = "IsLoggedIn";
    public const string LogoutRequest = "LogoutRequest";
    public AsyncCommand SubmitCommand { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public override void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);

        if (parameters.ContainsKey(LogoutRequest))
        {
            //do logout
            preferencesService.Value.Set(IsLoggedIn, false);
        }
    }

    private async Task OnSubmitCommand(object arg)
    {
        //nameof(OnSubmitCommand);

        preferencesService.Value.Set(IsLoggedIn, true);
        await Services.NavigationService.Navigate($"/{nameof(MoviesPageViewModel)}");
    }
}
