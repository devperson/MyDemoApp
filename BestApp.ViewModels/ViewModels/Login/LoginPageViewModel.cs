using Base.Abstractions.Platform;
using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Movies;

namespace BestApp.ViewModels.Login;

public class LoginPageViewModel : AppPageViewModel
{
    private readonly Lazy<IPreferencesService> preferencesService;

    public LoginPageViewModel(PageInjectedServices services, Lazy<IPreferencesService> preferencesService) : base(services)
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
