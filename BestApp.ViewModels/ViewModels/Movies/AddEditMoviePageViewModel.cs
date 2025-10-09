using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using BestApp.Abstraction.Main.UI;
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
        public const string UPDATE_ITEM = "updateItem";

        public AddEditMoviePageViewModel(InjectedServices services, 
                                      Lazy<IMovieService> movieService, 
                                      Lazy<IPopupAlert> popupAlert) : base(services)
        {
            SaveCommand = new AsyncCommand(OnSaveCommand);
            ChangePhotoCommand = new AsyncCommand(OnChangePhotoCommand);
            this.movieService = movieService;
            this.popupAlert = popupAlert;
        }

        public bool IsEdit { get; set; }        
        public AsyncCommand SaveCommand { get; set; }
        public AsyncCommand ChangePhotoCommand { get; set; }
        public MovieItemViewModel Model { get; set; }        

        public override void Initialize(Abstraction.Main.UI.Navigation.INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if(parameters.ContainsKey(MoviesPageViewModel.SELECTED_ITEM))
            {
                this.IsEdit = true;
                this.Model = parameters.GetValue<MovieItemViewModel>(MoviesPageViewModel.SELECTED_ITEM);
            }
            else
            {
                this.Model = new MovieItemViewModel();
            }
        }

        private async Task OnChangePhotoCommand(object arg)
        {
            
        }

        private async Task OnSaveCommand(object arg)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Model.Name))
                {
                    await popupAlert.Value.ShowError("The Name field is required");
                }
                else if (string.IsNullOrEmpty(this.Model.Description))
                {
                    await popupAlert.Value.ShowError("The Description field is required");
                }


                Some<MovieDto> result = null;
                if (this.IsEdit)
                {                    
                    result = await movieService.Value.Update(Model.Id, this.Model.Name, this.Model.Description, this.Model.PosterUrl);
                }
                else
                {
                    result = await movieService.Value.Add(this.Model.Name, this.Model.Description, this.Model.PosterUrl);
                }


                if (result.Success)
                {                    
                    var item = new MovieItemViewModel(result.Value);
                    var key = this.IsEdit ? UPDATE_ITEM : NEW_ITEM;
                    await NavigateBack(new NavigationParameters()
                    {
                        {key, item}
                    });
                }
                else
                {
                    await popupAlert.Value.ShowError(CommonStrings.GeneralError);
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
