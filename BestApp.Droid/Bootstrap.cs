using Base.Abstractions;
using Base.Abstractions.Diagnostic;
using Base.Abstractions.Platform;
using Base.Abstractions.UI;
using Base.Aspect;
using Base.MVVM.Navigation;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Login;
using BestApp.X.Droid.Pages.Movies;
using DryIoc;
using Mapster;
using MapsterMapper;

namespace BestApp.X.Droid
{    
    public class Bootstrap
    {
       // private ILoggingService loggingService;
       public IContainer container { get; set; }    
        public void RegisterTypes(IPageNavigationService pageNavigationService)
        {
            container = new Container(
                  Rules.Default.With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic).WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace));
            //register mapper
            var mapperConfig = new TypeAdapterConfig();
            container.RegisterInstance(mapperConfig);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            //register navigation service            
            container.RegisterInstance(pageNavigationService);
            container.Register<PageInjectedServices>();
            container.Register<IConstants, ConstantImpl>(Reuse.Singleton);

            //register app, infrastructure services
            // //register infrastructures            
            Base.Impl.Registrar.RegisterTypes(container);
            Base.Impl.Droid.Registrar.RegisterTypes(container);
            BestApp.Impl.Cross.Registrar.RegisterTypes(container, mapperConfig);
            BestApp.Impl.Droid.Registrar.RegisterTypes(container);

            var logger = container.Resolve<ILoggingService>();
            LogMethodsAttribute.LoggingService = logger;            

            //register ViewModel for navigation
            container.RegisterPageForNavigation<LoginPage, LoginPageViewModel>();
            container.RegisterPageForNavigation<MoviesPage, MoviesPageViewModel>();
            container.RegisterPageForNavigation<MovieDetailPage, MovieDetailPageViewModel>();
            container.RegisterPageForNavigation<AddEditMoviePage, AddEditMoviePageViewModel>();            
        }


        public async Task NavigateToPageAsync(IPageNavigationService pageNavigationService)
        {
            var preference = container.Resolve<IPreferencesService>();
            var isloggedIn = preference.Get(LoginPageViewModel.IsLoggedIn, false);

            if (isloggedIn)
            {
                await pageNavigationService.Navigate($"/{nameof(MoviesPageViewModel)}", animated: false);
            }
            else
            {
                await pageNavigationService.Navigate($"/{nameof(LoginPageViewModel)}", animated: false);
            }

            //await pageNavigationService.Navigate($"/{nameof(MoviesPageViewModel)}", animated: false);

            //this.loggingService = ContainerLocator.Resolve<ILoggingService>();
            //this.SubscribeToUnhandledErrors();

            //var appService = ContainerLocator.Resolve<AppService>();
            //appService.ResolveAppServer("Test");

            //var userService = ContainerLocator.Resolve<AccountService>();
            //userService.Init();
            //LogDeviceAppDetails(userService);

            //if (userService.HasAccount)
            //{
            //    await pageNavigationService.Navigate($"/{nameof(HomeViewModel)}", animated: false);
            //}
            //else
            //{
            //    await pageNavigationService.Navigate($"/{nameof(LoginViewModel)}", animated: false);
            //}
        }

        //private void LogDeviceAppDetails(AccountService userService)
        //{
        //    this.loggingService.Header($"\n********************************************************* \n" +
        //        $"     DATE: {DateTimeOffset.Now} \n" +
        //        $"********************************************************* \n");

        //    this.loggingService.Header($"\n********************************************************* \n" +
        //        $"     APP BUILD VERSION: {AppInfo.VersionString} ({AppInfo.BuildString}) \n" +
        //        $"********************************************************* \n");
        //    if (userService.HasAccount)
        //    {
        //        this.loggingService.Header($"\n********************************************************* \n" +
        //            $"      USERNAME: {userService.MyInfo.Name}, Email: {userService.MyInfo.Email}, ID: {userService.MyInfo.ID} \n" +
        //            $"********************************************************* \n");
        //    }

        //    var deviceService = ContainerLocator.Resolve<IDevice>();
        //    this.loggingService.Header($"\n********************************************************* \n" +
        //        $"      DEVICE NAME: {deviceService.DeviceInfo.Name} \n" +
        //        $"      PLATFORM: {deviceService.DeviceInfo.Platform} \n" +
        //        $"      OS VERSION: {deviceService.DeviceInfo.Version} \n" +
        //        $"      MODEL: {deviceService.DeviceInfo.Model} \n" +
        //        $"      MANUFACTURER: {deviceService.DeviceInfo.Manufacturer} \n" +
        //        $"      IDIOM: {deviceService.DeviceInfo.Idiom} \n" +
        //        $"      DEVICE TYPE: {deviceService.DeviceInfo.DeviceType} \n" +
        //        $"      OS VERSION STRING: {deviceService.DeviceInfo.VersionString} \n" +
        //        $"********************************************************* \n");

        //    //log local db folder info
        //    var directoryService = ContainerLocator.Resolve<IDirectoryService>();
        //    var dbFolderInfo = directoryService.GetLogInfoForDbDirectory();
        //    this.loggingService.Header($"\n********************************************************* \n" +
        //                              $"{dbFolderInfo} \n"+
        //                              $"********************************************************* \n");
        //}


    }
}
