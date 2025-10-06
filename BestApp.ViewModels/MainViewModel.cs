using BestApp.Abstraction.Common;
using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.Infasructures;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.ItemViewModel;
using Logging.Aspects;
using System.Collections.ObjectModel;

namespace BestApp.ViewModels
{
    [LogMethods]
    public class MainViewModel : PageViewModel
    {        
        private readonly Lazy<IMovieService> movieService;
        private readonly Lazy<IInfrastructureServices> infrastructureServices;

        public MainViewModel(InjectedServices services, Lazy<IMovieService> movieService, Lazy<IInfrastructureServices> infrastructureServices) : base(services)
        {
            
            this.movieService = movieService;
            this.infrastructureServices = infrastructureServices;
            AddCommand = new AsyncCommand(OnAddCommand);
            ItemSelectedCommand = new AsyncCommand(OnItemSelectedCommand);
        }

        

        public ObservableCollection<MovieItemViewModel> MovieItems { get; set; }
        public AsyncCommand AddCommand { get; set; }
        public AsyncCommand ItemSelectedCommand { get; set; }

        public override void Initialize(Abstraction.General.Platform.INavigationParameters parameters)
        {
            base.Initialize(parameters);
                        
            //init infrastructure services (ie local storage, rest api)
            infrastructureServices.Value.Start();
        }

        public override void PausedToBackground()
        {
            infrastructureServices.Value.Pause();
        }

        public override void ResumedFromBackground()
        {
            infrastructureServices.Value.Resume();
        }

        public override void Destroy()
        {
            infrastructureServices.Value.Stop();
        }

        public override async void OnFirstTimeAppears()
        {
            base.OnFirstTimeAppears();

            await ShowLoadingAndHandleError(async () =>
            {
                await LoadData();
            });
        }

       

        public override void OnNavigatedTo(Abstraction.General.Platform.INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.ContainsKey(CreateMovieViewModel.NEW_ITEM))
                {
                    var newProduct = parameters.GetValue<MovieItemViewModel>(CreateMovieViewModel.NEW_ITEM);
                    MovieItems.Add(newProduct);
                }
            }
            catch (Exception ex)
            {
                Services.LoggingService.TrackError(ex);
            }
        }

        protected override async Task OnRefreshCommand(object arg)
        {
            await ShowLoadingAndHandleError(async () =>
            {
                await LoadData(remoteList: true);
            });
        }

        private async Task OnAddCommand(object arg)
        {
            try
            {
                await Navigate(nameof(MovieItemViewModel));
            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private Task OnItemSelectedCommand(object arg)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                HandleUIError(ex);
            }

            return Task.CompletedTask;
        }


        public async Task LoadData(bool remoteList = false)
        {
            var result = await movieService.Value.GetList(remoteList: remoteList);
            if (result.Success)
            {
                var list = result.Value.Select(x => new MovieItemViewModel(x));
                MovieItems = new ObservableCollection<MovieItemViewModel>(list);
            }            
        }


    }
}
