using Base.Aspect;
using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using BestApp.Abstraction.Main.AppService;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Events;
using BestApp.ViewModels.Movies.ItemViewModel;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Movies;

[LogMethods]
public class MovieDetailPageViewModel : AppPageViewModel
{
    public const string SELECTED_ITEM = "selectedItem";
    protected readonly Lazy<IMoviesService> moviesService;

    public MovieDetailPageViewModel(PageInjectedServices services, Lazy<IMoviesService> moviesService) : base(services)
    {
        this.EditCommand = new AsyncCommand(OnEditCommand);
        this.moviesService = moviesService;
    }

    public MovieItemViewModel Model { get; set; }
    public AsyncCommand EditCommand { get; set; }

    public override async void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);

        if (parameters.ContainsKey(SELECTED_ITEM))
        {
            var movieId = parameters.GetValue<int>(SELECTED_ITEM);
            await LoadMovie(movieId);
        }
    }

    

    public override async void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);

        if (parameters.ContainsKey(AddEditMoviePageViewModel.UPDATED_ITEM))
        {
            await LoadMovie(this.Model.Id);
            var itemUpdatedEvent = Services.EventAggregator.GetEvent<MovieCellItemUpdatedEvent>();
            itemUpdatedEvent.Publish(this.Model.Id);
        }
    }

    private async Task OnEditCommand(object arg)
    {
        try
        {            
            await this.Navigate(nameof(AddEditMoviePageViewModel), new NavigationParameters 
            { 
                { AddEditMoviePageViewModel.SELECTED_ITEM, this.Model.Id } 
            });
        }
        catch (Exception ex)
        {
            HandleUIError(ex);
        }
    }


    protected async Task LoadMovie(int movieId)
    {
        var result = await moviesService.Value.GetById(movieId);
        if (result.Success)
        {
            this.Model = new MovieItemViewModel(result.Value);
        }
    }
}
