using Base.MVVM.Helper;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Movies;

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
        //nameof(OnSubmitCommand);
        await Services.NavigationService.Navigate($"/{nameof(MoviesPageViewModel)}");
    }
}
