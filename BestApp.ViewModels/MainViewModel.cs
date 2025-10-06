using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Infasructures.Events;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.ItemViewModel;
using Logging.Aspects;
using System.Collections.ObjectModel;
using BestApp.ViewModels.Extensions;

namespace BestApp.ViewModels
{
    [LogMethods]
    public class MainViewModel : PageViewModel
    {        
        private readonly Lazy<IMovieService> movieService;
        private readonly Lazy<IInfrastructureServices> infrastructureServices;
        private AuthErrorEvent authErrorEvent;

        public MainViewModel(InjectedServices services, Lazy<IMovieService> movieService, Lazy<IInfrastructureServices> infrastructureServices) : base(services)
        {
            
            this.movieService = movieService;
            this.infrastructureServices = infrastructureServices;
            AddCommand = new AsyncCommand(OnAddCommand);
            ItemSelectedCommand = new AsyncCommand(OnItemSelectedCommand);

            authErrorEvent = services.EventAggregator.GetEvent<AuthErrorEvent>();
            authErrorEvent.Subscribe(HandleAuthErrorEvent);
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
                await Navigate(nameof(CreateMovieViewModel));
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

        private readonly SemaphoreSlim semaphoreAuthError = new(1, 1);
        private bool loggingOut = false;
        private async void HandleAuthErrorEvent()
        {
            try
            {
                await semaphoreAuthError.WaitAsync();
                if (loggingOut)
                {
                    // we have already handling this event, so we can just ignore this thread
                    Services.LoggingService.LogWarning("Skip HandleAuthErrorEvent() because it is handled by other thread (semaphoreAuthError.Wait(0) == false)");
                    return;
                }
                loggingOut = true;

                var currentPageViewModel = GetCurrentPageViewModel();
                if (!(currentPageViewModel is MainViewModel))
                    await currentPageViewModel.NavigateToRoot();

                ////force to log out
                //this.LoadingText = "Logging out...";
                //BusyLoading = true;
                //var loginVm = Service.Container.Resolve<LoginViewModel>();
                //bool logOut = false;
                //while (!logOut)
                //{
                //    try
                //    {
                //        await loginVm.SignOut();
                //        logOut = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        Service.LoggingService.LogWarning($"{TAG}Failed to log out. Attempt to log out again. Exception details: {ex}");
                //        await Task.Delay(800);
                //    }
                //}

                //string logoutMessage = string.Empty;
                //if (reason == LogOutReasonType.ServerRequested)
                //    logoutMessage = "You're currently signed in on another device. For your security, this device has been signed out.";
                ////run this in the main thread
                //await Navigate($"../{nameof(LoginViewModel)}", new NavigationParameters()
                //            {
                //                {LoginViewModel.SKIP_CLEARING, true },
                //                {LoginViewModel.LOG_OUT_MESSAGE, logoutMessage },
                //            });
            }
            catch (Exception ex)
            {
                Services.LoggingService.TrackError(ex);
            }
            finally
            {
                semaphoreAuthError.SecureRelease();
            }
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
