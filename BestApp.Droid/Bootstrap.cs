using BestApp.Abstraction.Common;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Login;
using BestApp.X.Droid.Pages.Movies;
using BestApp.X.Droid.Utils;
using Common.Abstrtactions;
using Logging.Aspects;
using DryIoc;
using KYChat.Controls.Navigation;
using System.Globalization;
using BestApp.Abstraction.Main.UI.Navigation;
using BestApp.ViewModels.Base;
using Prism.Ioc;
using Mapster;
using MapsterMapper;

namespace BestApp.X.Droid
{    
    public class Bootstrap
    {
       // private ILoggingService loggingService;

        public void RegisterTypes(IPageNavigationService pageNavigationService)
        {
            var container = DryIocContainerExtension.CreateInstance();
            ContainerLocator.SetContainerExtension(container);
            //register mapper
            var mapperConfig = new TypeAdapterConfig();
            container.RegisterInstance(mapperConfig);
            // Register Mapster's service
            container.RegisterSingleton<IMapper, Mapper>();

            //register navigation service            
            container.RegisterInstance(pageNavigationService);
            container.Register<InjectedServices>();            
            container.RegisterSingleton<IConstants, ConstantImpl>();

            //register app, infrastructure services            
            var dryIocContainer = (DryIocContainerExtension)container;
            Impl.Cross.Registerar.RegisterTypes(dryIocContainer.Instance, mapperConfig);
            Impl.Droid.Registerar.RegisterTypes(dryIocContainer.Instance, mapperConfig);
            LogMethodsAttribute.LoggingService = container.Resolve<ILoggingService>();            

            //register ViewModel for navigation
            container.RegisterPageForNavigation<LoginPage, LoginPageViewModel>();
            container.RegisterPageForNavigation<MoviesPage, MoviesPageViewModel>();
            container.RegisterPageForNavigation<MovieDetailPage, MovieDetailPageViewModel>();
            container.RegisterPageForNavigation<AddEditMoviePage, AddEditMoviePageViewModel>();            
        }


        public async Task NavigateToPageAsync(IPageNavigationService pageNavigationService)
        {
            await pageNavigationService.Navigate($"/{nameof(LoginPageViewModel)}", animated: false);

            //await pageNavigationService.Navigate($"/{nameof(MoviesPageViewModel)}", animated: false);

            //this.loggingService = ContainerLocator.Container.Resolve<ILoggingService>();
            //this.SubscribeToUnhandledErrors();

            //var appService = ContainerLocator.Container.Resolve<AppService>();
            //appService.ResolveAppServer("Test");

            //var userService = ContainerLocator.Container.Resolve<AccountService>();
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

        //    var deviceService = ContainerLocator.Container.Resolve<IDevice>();
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
        //    var directoryService = ContainerLocator.Container.Resolve<IDirectoryService>();
        //    var dbFolderInfo = directoryService.GetLogInfoForDbDirectory();
        //    this.loggingService.Header($"\n********************************************************* \n" +
        //                              $"{dbFolderInfo} \n"+
        //                              $"********************************************************* \n");
        //}


    }
}
