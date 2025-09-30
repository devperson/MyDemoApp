

using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Common.Abstrtactions;
using Microsoft.Maui.ApplicationModel;

namespace BestApp.X.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        private ViewGroup? layoutRoot;
        public PageNavigationFrameLayout pageNavigationService;
        private ILoggingService loggingService;

        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}