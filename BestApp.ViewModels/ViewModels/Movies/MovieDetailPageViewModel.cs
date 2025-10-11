using Base.Aspect;
using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Events;
using BestApp.ViewModels.Movies.ItemViewModel;

namespace BestApp.ViewModels.Movies;

[LogMethods]
public class MovieDetailPageViewModel : PageViewModel
{
    public MovieDetailPageViewModel(InjectedServices services) : base(services)
    {
        this.EditCommand = new AsyncCommand(OnEditCommand);
    }

    public MovieItemViewModel Model { get; set; }
    public AsyncCommand EditCommand { get; set; }

    public override void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);

        if (parameters.ContainsKey(MoviesPageViewModel.SELECTED_ITEM))
        {
            this.Model = parameters.GetValue<MovieItemViewModel>(MoviesPageViewModel.SELECTED_ITEM);
        }        
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        if (parameters.ContainsKey(AddEditMoviePageViewModel.UPDATE_ITEM))
        {
            this.Model = parameters.GetValue<MovieItemViewModel>(AddEditMoviePageViewModel.UPDATE_ITEM);
            Services.EventAggregator.GetEvent<MovieCellItemUpdatedEvent>().Publish(this.Model);
        }
    }

    private async Task OnEditCommand(object arg)
    {
        try
        {            
            await this.Navigate(nameof(AddEditMoviePageViewModel), new NavigationParameters { { MoviesPageViewModel.SELECTED_ITEM, this.Model } });
        }
        catch (Exception ex)
        {
            HandleUIError(ex);
        }
    }
}
