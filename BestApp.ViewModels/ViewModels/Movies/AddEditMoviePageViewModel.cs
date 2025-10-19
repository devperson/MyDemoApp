using Base.Abstractions.AppService;
using Base.Abstractions.UI;
using Base.Aspect;
using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Helper;
using BestApp.ViewModels.Movies.ItemViewModel;

namespace BestApp.ViewModels.Movies
{
    [LogMethods]
    public class AddEditMoviePageViewModel : AppPageViewModel
    {
        private readonly Lazy<IMoviesService> movieService;
        private readonly Lazy<IMediaPickerService> mediaPickerService;
        public const string NEW_ITEM = "newItem";
        public const string UPDATE_ITEM = "updateItem";
        public const string REMOVE_ITEM = "removeItem";

        public AddEditMoviePageViewModel(PageInjectedServices services, 
                                      Lazy<IMoviesService> movieService,
                                      Lazy<IMediaPickerService> mediaPickerService) : base(services)
        {            
            this.movieService = movieService;
            this.mediaPickerService = mediaPickerService; 
            SaveCommand = new AsyncCommand(OnSaveCommand);
            ChangePhotoCommand = new AsyncCommand(OnChangePhotoCommand);
            DeleteCommand = new AsyncCommand(OnDeleteCommand);
        }
        
        public bool IsEdit { get; set; }        
        public AsyncCommand SaveCommand { get; set; }
        public AsyncCommand ChangePhotoCommand { get; set; }
        public AsyncCommand DeleteCommand { get; set; }
        public MovieItemViewModel Model { get; set; }        

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if(parameters.ContainsKey(MoviesPageViewModel.SELECTED_ITEM))
            {
                this.IsEdit = true;
                this.Model = parameters.GetValue<MovieItemViewModel>(MoviesPageViewModel.SELECTED_ITEM);
                this.Title = "Edit";
            }
            else
            {
                this.Model = new MovieItemViewModel();
                this.Title = "Add new";
            }
        }

        private async Task OnChangePhotoCommand(object arg)
        {
            try
            {
                var deleteText = !string.IsNullOrEmpty(this.Model.PosterUrl) ? "Delete" : null;
                var buttons = new[] { "Pick Photo", "Take Photo" };
                var actionResult = await Services.AlerDialogService.DisplayActionSheet("Set photo from", "Cancel", deleteText, buttons);

                if (actionResult == buttons[0])
                {
                    var photo = await mediaPickerService.Value.GetPhotoAsync(new PhotoOptions() { ShrinkPhoto = false });
                    this.Model.PosterUrl = photo.FilePath;
                }
                else if (actionResult == buttons[1])
                {
                    var photo = await mediaPickerService.Value.TakePhotoAsync(new PhotoOptions() { ShrinkPhoto = false, WithFilePath = true });
                    this.Model.PosterUrl = photo.FilePath;
                }
                else if (actionResult == deleteText)
                {
                    this.Model.PosterUrl = null;
                }                
            }           
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private async Task OnDeleteCommand(object arg)
        {
            try
            {
                var res = await Services.AlerDialogService.ConfirmAlert("Confirm", "Are you sure you want to delete this item?", "Yes", "No");

                if (res == true)
                {                    
                    var dtoModel = Model.ToDto();
                    var result = await movieService.Value.RemoveAsync(dtoModel);

                    if (result.Success)
                    {                                                
                        await NavigateToRoot(new NavigationParameters()
                        {
                            { REMOVE_ITEM, this.Model }
                        });
                    }
                    else
                    {
                        Services.SnackBarService.ShowError(CommonStrings.GeneralError);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private async Task OnSaveCommand(object arg)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Model.Name))
                {
                    Services.SnackBarService.ShowError("The Name field is required");
                    return;
                }
                else if (string.IsNullOrEmpty(this.Model.Overview))
                {
                    Services.SnackBarService.ShowError("The Overview field is required");
                    return;
                }

                Some<MovieDto> result = null;
                if (this.IsEdit)
                {                    
                    var dtoModel = this.Model.ToDto();
                    result = await movieService.Value.UpdateAsync(dtoModel);
                }
                else
                {
                    result = await movieService.Value.AddAsync(this.Model.Name, this.Model.Overview, this.Model.PosterUrl);
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
                    Services.SnackBarService.ShowError(CommonStrings.GeneralError);
                }                   
            }                        
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }
    }
}
