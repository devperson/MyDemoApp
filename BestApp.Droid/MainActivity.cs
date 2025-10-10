using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using BestApp.ViewModels.Base;
using BestApp.X.Droid.Pages.Base;
using BestApp.X.Droid.Controls.Navigation;
using Common.Abstrtactions;
using Microsoft.Maui.ApplicationModel;
using System.Globalization;
using BestApp.X.Droid.Controls;
using Google.Android.Material.SideSheet;

namespace BestApp.X.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        private ViewGroup? layoutRoot;
        private MainSideSheetDialog sideSheetDialog;
        public PageNavigationFrameLayout pageNavigationService;
        private ILoggingService loggingService;

        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetCulture();
            Platform.Init(this, savedInstanceState);
            Instance = this;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            layoutRoot = this.FindViewById<ViewGroup>(Resource.Id.layoutRoot);

            //setup navigation
            pageNavigationService = this.FindViewById<PageNavigationFrameLayout>(Resource.Id.navContainer);
            pageNavigationService.SetActivity(this);

            //register services
            var bootstrap = new Bootstrap();
            bootstrap.RegisterTypes(pageNavigationService);


            this.loggingService = ContainerLocator.Container.Resolve<ILoggingService>();

            this.loggingService.Log("####################################################- APPLICATION STARTED -####################################################");
            this.loggingService.Log($"MainActivity.OnCreate()");

            await bootstrap.NavigateToPageAsync(pageNavigationService);
        }

        // when user click on page we should hide keyboard
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            try
            {
                var dispatchEventListener = pageNavigationService.currentPage as IDispatchEventListener;
                if (dispatchEventListener != null)
                {
                    dispatchEventListener.DispatchTouchEvent(ev);
                }              

                return base.DispatchTouchEvent(ev); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                return true;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            //This method is called when Device's system back button is pressed (which is in bottom bar in Android)
            //We need to check if it is not a root page because we don't want to pop last page
            if (pageNavigationService.CanNavigateBack)
            {
                var currentPage = pageNavigationService.GetCurrentPage();
                if (currentPage != null)
                {
                    //We need to do Pop navigation only when Push navigation animation is completed.
                    //This prevents bugs such as https://github.com/imtllc/utilla-app-QA/issues/2531#event-17787173104              
                    //It happens when user navigate to some page and tap on back system button quickly while push animation still in progress
                    //The fix is to ignore back button while page push animation in progress                   
                    if (currentPage.IsPageEnterAnimationCompleted)
                    {
                        //push animation is not in progress so we can do Pop navigation
                        var currentPageVm = currentPage.ViewModel;
                        if (currentPageVm != null)
                        {
                            currentPageVm.DoDeviceBackCommand();
                        }
                    }
                }
            }
            else
            {
                var currentVm = this.GetCurrentViewModel();
                loggingService?.LogWarning($"MainActivity.OnBackPressed() is canceled because CanNavigateBack is false for current page. Seems current page is root page thus can not navigate back, page: {currentVm}");
            }
        }

        private static void SetCulture()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
            CultureInfo.DefaultThreadCurrentUICulture = englishUSCulture;
        }

        public PageViewModel GetCurrentViewModel()
        {
            var vm = this.pageNavigationService.GetCurrentPageModel() as PageViewModel;
            return vm;
        }

        public PageViewModel GetRootPageViewModel()
        {
            var vm = this.pageNavigationService.GetRootPageModel() as PageViewModel;
            return vm;
        }

        public LifecyclePage GetCurrentPage()
        {
            return this.pageNavigationService.GetCurrentPage();
        }

        public void ShowSideSheet()
        {
            if (this.sideSheetDialog == null)
            {
                this.sideSheetDialog = new MainSideSheetDialog(this);
                this.sideSheetDialog.SetContentView(Resource.Layout.page_main_sidesheet_view);
                this.sideSheetDialog.SetSheetEdge((int)GravityFlags.Start);
            }

            sideSheetDialog.Show();
        }
    }
}