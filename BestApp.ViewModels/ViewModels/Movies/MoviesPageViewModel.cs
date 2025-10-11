using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Movies.ItemViewModel;
using System.Collections.ObjectModel;
using BestApp.ViewModels.Extensions;
using BestApp.ViewModels.Events;
using Base.Abstractions.UI;
using Base.Abstractions.REST;
using Base.Aspect;
using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using System.Runtime.Serialization;
using BestApp.ViewModels.Login;

namespace BestApp.ViewModels.Movies
{
    [LogMethods]
    public class MoviesPageViewModel : PageViewModel
    {        
        private readonly Lazy<IMoviesService> movieService;
        private readonly Lazy<IAlertDialogService> alertDialogService;
        private readonly Lazy<IInfrastructureServices> infrastructureServices;
        private readonly Lazy<ISnackbarService> snackbarService;
        private AuthErrorEvent authErrorEvent;
        public const string SELECTED_ITEM = "selectedItem";

        public MoviesPageViewModel(InjectedServices services, 
            Lazy<IMoviesService> movieService, 
            Lazy<IAlertDialogService> alertDialogService,
            Lazy<IInfrastructureServices> infrastructureServices,
            Lazy<ISnackbarService> snackbarService) : base(services)
        {
            
            this.movieService = movieService;
            this.alertDialogService = alertDialogService;
            this.infrastructureServices = infrastructureServices;
            this.snackbarService = snackbarService;
            AddCommand = new AsyncCommand(OnAddCommand);
            ItemTappedCommand = new AsyncCommand(OnItemTappedCommand);
            MenuTappedCommand = new AsyncCommand(OnMenuTappedCommand);

            authErrorEvent = services.EventAggregator.GetEvent<AuthErrorEvent>();
            authErrorEvent.Subscribe(HandleAuthErrorEvent);
        }

        public ObservableCollection<MovieItemViewModel> MovieItems { get; set; }
        public AsyncCommand MenuTappedCommand { get; }
        public AsyncCommand AddCommand { get; set; }
        public AsyncCommand ItemTappedCommand { get; set; }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            this.Services.EventAggregator.GetEvent<MovieCellItemUpdatedEvent>().Subscribe(OnMovieCellItemUpdatedEvent);
                        
            //init infrastructure services (ie local storage, rest api)
            await infrastructureServices.Value.Start();
            
            await ShowLoadingAndHandleError(async () =>
            {
                await LoadData();
            });
        }        

        public override void PausedToBackground(object arg)
        {
            infrastructureServices.Value.Pause();
        }

        public override void ResumedFromBackground(object arg)
        {
            infrastructureServices.Value.Resume();
        }

        public override void Destroy()
        {
            base.Destroy();

            infrastructureServices.Value.Stop();

            this.Services.EventAggregator.GetEvent<MovieCellItemUpdatedEvent>().Unsubscribe(OnMovieCellItemUpdatedEvent);
        }

        
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.ContainsKey(AddEditMoviePageViewModel.NEW_ITEM))
                {
                    var newProduct = parameters.GetValue<MovieItemViewModel>(AddEditMoviePageViewModel.NEW_ITEM);
                    MovieItems.Insert(0, newProduct);
                }
                else if (parameters.ContainsKey(AddEditMoviePageViewModel.REMOVE_ITEM))
                {
                    var removeItem = parameters.GetValue<MovieItemViewModel>(AddEditMoviePageViewModel.REMOVE_ITEM);
                    MovieItems.Remove(removeItem);
                }

            }
            catch (Exception ex)
            {
                Services.LoggingService.TrackError(ex);
            }
        }

        private async Task OnMenuTappedCommand(object obj)
        {
            var item = obj as MenuItem;           
            if (item.Type == MenuType.Logout)
            {
                var res = await alertDialogService.Value.ConfirmAlert("Confirm Action", "Are you sure want to log out?", "Yes", "No");
                if (res)
                {                    
                    await Navigate($"../{nameof(LoginPageViewModel)}", new NavigationParameters { { LoginPageViewModel.LogoutRequest, true} });
                }
            }
        }

        private void OnMovieCellItemUpdatedEvent(MovieItemViewModel model)
        {
            var oldItem = this.MovieItems.FirstOrDefault(m => m.Id == model.Id);
            var index = this.MovieItems.IndexOf(oldItem);

            //this will raise Replace event for ObservableCollection and views will listen for it.
            this.MovieItems[index] = model;
        }

        protected override async Task OnRefreshCommand(object arg)
        {
            IsRefreshing = true;

            await ShowLoadingAndHandleError(async () =>
            {
                await LoadData(remoteList: true);
            }, setIsBusy: false);

            IsRefreshing = false;
        }

        private async Task OnAddCommand(object arg)
        {
            try
            {
                await Navigate(nameof(AddEditMoviePageViewModel));
            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private async Task OnItemTappedCommand(object arg)
        {
            try
            {
                var item = arg as MovieItemViewModel;
                await this.Navigate(nameof(MovieDetailPageViewModel), new NavigationParameters { { SELECTED_ITEM, item } });
            }
            catch(Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private readonly SemaphoreSlim semaphoreAuthError = new(1, 1);
        private bool loggingOut = false;
        private async void HandleAuthErrorEvent(object arg)
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
                if (!(currentPageViewModel is MoviesPageViewModel))
                    await currentPageViewModel.NavigateToRoot(new NavigationParameters());

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
            var result = await movieService.Value.GetListAsync(remoteList: remoteList);
            if (result.Success)
            {
                var list = result.Value.Select(x => new MovieItemViewModel(x));
                MovieItems = new ObservableCollection<MovieItemViewModel>(list);
            }      
            else
            {
                snackbarService.Value.Show(result.Exception.ToString());
            }
        }


    }

    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public MenuType Type { get; set; }
    }

    public enum MenuType
    {              
        Logout = 7,        
        Settings = 8
    }
}
