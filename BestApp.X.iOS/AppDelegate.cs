using Base.Abstractions.Diagnostic;
using Base.Impl.iOS.UI.Navigation;
using BestApp.X.iOS.Pages.Movies;
using DryIoc;
using KYChat.iOS.Controls;
using System.Globalization;

namespace BestApp.X.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public static AppDelegate Instance { get; private set; }

        public iOSPageNavigationController pageNavigationService;
        public FlyoutController flyoutController;
        private SideMenuController sideViewController;
        private Bootstrap bootstrap;
        public IContainer Container { get; set; }
        private ILoggingService loggingService;

        public override UIWindow? Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            SetCulture();
            Instance = this;

            pageNavigationService = new iOSPageNavigationController();
            bootstrap = new Bootstrap();
            bootstrap.RegisterTypes(pageNavigationService);
            Container = bootstrap.container;

            this.loggingService = Container.Resolve<ILoggingService>();

            this.loggingService.Log("####################################################- APPLICATION STARTED -####################################################");
            this.loggingService.Log($"MainActivity.OnCreate()");

            _ = bootstrap.NavigateToPageAsync(pageNavigationService);

            this.sideViewController = new SideMenuController();
            this.flyoutController = new FlyoutController(this.pageNavigationService, this.sideViewController, null);

            Window.RootViewController = this.flyoutController;

            // make the window visible
            Window.MakeKeyAndVisible();

            return true;
        }

        private static void SetCulture()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
            CultureInfo.DefaultThreadCurrentUICulture = englishUSCulture;
        }
    }
}
