using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.UI;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Helper;
using BestApp.ViewModels.Helper.Commands;
using BestApp.ViewModels.Movies.ItemViewModel;
using Logging.Aspects;

namespace BestApp.ViewModels.Movies
{
    [LogMethods]
    public class AddEditMoviePageViewModel : PageViewModel
    {
        private readonly Lazy<IMovieService> movieService;
        private readonly Lazy<IPopupAlert> popupAlert;
        public const string NEW_ITEM = "newItem";

        public AddEditMoviePageViewModel(InjectedServices services, 
                                      Lazy<IMovieService> movieService, 
                                      Lazy<IPopupAlert> popupAlert) : base(services)
        {
            CreateCommand = new AsyncCommand(OnCreateCommand);
            this.movieService = movieService;
            this.popupAlert = popupAlert;
        }

        

        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterImage { get; set; }

        public AsyncCommand CreateCommand { get; set; }

        private async Task OnCreateCommand(object arg)
        {
            try
            {                

                var result = await movieService.Value.Add(Name, Overview, PosterImage);
                if(result.Success)
                {
                    var movie = result.Value;
                    var productItem = new MovieItemViewModel(movie);

                    await NavigateBack(new NavigationParameters()
                    {
                        {NEW_ITEM, productItem}
                    });
                }
                else
                {
                    if (result.Exception is ArgumentException)
                    {
                        if (result.Exception.Message.Contains("name"))
                            await popupAlert.Value.ShowError("The Name field is required");
                        else if (result.Exception.Message.Contains("overview"))
                            await popupAlert.Value.ShowError("The Overview field is required");
                    }
                    else
                    {
                        await popupAlert.Value.ShowError(CommonStrings.GeneralError);
                    }
                }                
            }                        
            catch (Exception ex)
            {
                await popupAlert.Value.ShowError(CommonStrings.GeneralError);
                Services.LoggingService.TrackError(ex);
            }
        }
    }
}
