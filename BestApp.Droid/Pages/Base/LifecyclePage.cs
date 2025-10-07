using Android.OS;
using Android.Views.Animations;
using Android.Views;
using AndroidX.Core.View;
using BestApp.X.Droid.Utils;

namespace BestApp.X.Droid.Pages.Base
{
    public class LifecyclePage : BasePage, IOnApplyWindowInsetsListener
    {
        protected TextView txtTitle;
        protected Button btnBack;
        private bool isVisible, onApprearedSent = false;
        //private SeverityType severity;
        private FrameLayout loadingView;
        private TextView txtLoadingMsg;
        private PageEnterAnimationListner pageAnimationListener;
        //private ILogging pageLogger;//conditional logger: that will log if user enables it
        private ViewGroup rootLayout;

        public bool IsPageEnterAnimationCompleted { get; set; }

        protected const string TitleProp = "Title";
        private const string BusyLoadingProp = "BusyLoading", ToastSeverityProp = "ToastSeverity", ToastMessageProp = "ToastMessage";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            loggingService.Log($"{GetType().Name}.OnCreate() (from base)");

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            loggingService.Log($"{GetType().Name}.OnViewCreated() (from base)");

            base.OnViewCreated(view, savedInstanceState);

            //this.txtTitle = view.FindViewById<TextView>(Resource.Id.txtTitle);
            this.btnBack = view.FindViewById<Button>(Resource.Id.btnBack);
            if (this.btnBack != null)
            {
                this.btnBack.Visibility = ViewModel.CanGoBack.ToVisibility();
                if (this.btnBack.Visibility == ViewStates.Visible)
                {
                    this.btnBack.Click += BtnBack_Click;
                }
            }

            //add loading indicator            
            //this.loadingView = (FrameLayout)LayoutInflater.From(this.Context).Inflate(Resource.Layout.fragment_loading_indicator, view as ViewGroup, false);
            //this.loadingView.Visibility = ViewStates.Gone;
            //this.txtLoadingMsg = this.loadingView.FindViewById<TextView>(Resource.Id.txtLoadingMsg);

            //set visibility for busy indicator via propertyChanged handler
            //this will check the model and set correct visibility for busy indicator            
            ViewModel_PropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs(BusyLoadingProp));
            //pageLogger = loggingService.CreateSpecificLogger(AdvancedLogConstants.LogPageInsets);

            rootLayout = view as ViewGroup;
            if (rootLayout is ScrollView)
            {
                var scrollView = rootLayout as ScrollView;
                if (scrollView.ChildCount > 0)
                    rootLayout = scrollView.GetChildAt(0) as ViewGroup;
            }

            //if (rootLayout != null)
            //{
            //    rootLayout.AddView(loadingView);
            //}

            HandleSoftInput();
        }

        /// <summary>
        /// Used for Android 15>, older Android versions take a look IKeyboardResizeTypeService
        /// </summary>
        private void HandleSoftInput()
        {
            //starting from Android 15 we need to add
            //padding top/bottom so to not overlay the status/navigation bar
            if (Build.VERSION.SdkInt >= BuildVersionCodes.VanillaIceCream)
            {
                // Apply padding dynamically to your root layout            
                ViewCompat.SetOnApplyWindowInsetsListener(rootLayout, this);
            }
        }

        public virtual WindowInsetsCompat OnApplyWindowInsets(View v, WindowInsetsCompat insets)
        {
            return ApplyStandartPadding(v, insets);
        }

        /// <summary>
        /// Sets top\bottom padding to page: to not overlay the status bar, bottom navigation bar
        /// </summary>
        protected WindowInsetsCompat ApplyStandartPadding(View v, WindowInsetsCompat insets)
        {
            //get navigation bar insets
            var navBarInsets = insets.GetInsets(WindowInsetsCompat.Type.NavigationBars());

            //get status bar insets
            var statusBarInsets = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());            

            //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: statusBarInsets:{{{statusBarInsets.Left},{statusBarInsets.Top},{statusBarInsets.Right},{statusBarInsets.Bottom}}}");
            //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: navBarInsets:{{{navBarInsets.Left},{navBarInsets.Top},{navBarInsets.Right},{navBarInsets.Bottom}}}");
            //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: appTopPadding:{appTopPadding}");
            //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: Density:{Resources.DisplayMetrics.Density}");
            //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: statusBarHeightDp:{statusBarInsets.Top / Resources.DisplayMetrics.Density}");
            if (statusBarInsets.Top > 0) //no need to set on old android  < Android 15 (on old Android bellow 15 it will be 0)
            {
                v.SetPadding(v.PaddingLeft, statusBarInsets.Top, v.PaddingRight, navBarInsets.Bottom);
                //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: Root layout padding:{{{v.PaddingLeft},{69},{v.PaddingRight},{navBarInsets.Bottom}}}");
            }
            else
            {
                //pageLogger.Log($"LifecyclePage.ApplyStandartPadding: Skip setting padding because top padding zero}}");
            }

            return insets;
        }

        /// <summary>
        /// Makes the same thing like ApplyStandartPadding() method but also adds additional 
        /// bottom padding when Keyboard appears to place any inputs/views above keyboard
        /// This only works for Android 15 > 
        /// </summary>
        protected WindowInsetsCompat ApplyPaddingWithKeyboard(View v, WindowInsetsCompat insets)
        {
            // Insets for navigation/status bars
            var navBarInsets = insets.GetInsets(WindowInsetsCompat.Type.NavigationBars());
            var statusBarInsets = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());

            // Insets for the on-screen keyboard (IME)
            var imeInsets = insets.GetInsets(WindowInsetsCompat.Type.Ime());
            // the layout also has own padding top which used for Older(bellow Android 15).We need to remove it from final top padding            
            //pageLogger.Log($"LifecyclePage.ApplyPaddingWithKeyboard: statusBarInsets:{{{statusBarInsets.Left},{statusBarInsets.Top},{statusBarInsets.Right},{statusBarInsets.Bottom}}}");
            //pageLogger.Log($"LifecyclePage.ApplyPaddingWithKeyboard: navBarInsets:{{{navBarInsets.Left},{navBarInsets.Top},{navBarInsets.Right},{navBarInsets.Bottom}}}");
            //pageLogger.Log($"LifecyclePage.ApplyPaddingWithKeyboard: imeInsets:{{{imeInsets.Left},{imeInsets.Top},{imeInsets.Right},{imeInsets.Bottom}}}");


            if (statusBarInsets.Top > 0) //no need to set on old android  < Android 15 (it will be zero on old Androids)
            {
                var selectedBottomPadding = Math.Max(navBarInsets.Bottom, imeInsets.Bottom);
                var customTopPadding = statusBarInsets.Top;
                // Apply top, bottom, left, right padding
                v.SetPadding(
                    v.PaddingLeft,
                    customTopPadding,
                    v.PaddingRight,
                    selectedBottomPadding); // choose max to handle keyboard
                //pageLogger.Log($"LifecyclePage.ApplyPaddingWithKeyboard: Root layout padding:{{{v.PaddingLeft},{statusBarInsets.Top},{v.PaddingRight},{selectedBottomPadding}}}");
            }

            return insets;
        }

        public override Animation OnCreateAnimation(int transit, bool enter, int nextAnim)
        {
            loggingService.Log($"{GetType().Name}.OnCreateAnimation() (from base)");

            if (enter && IsPageEnterAnimationCompleted == false && nextAnim > 0)
            {
                pageAnimationListener = new PageEnterAnimationListner(this);
                var animation = AnimationUtils.LoadAnimation(MainActivity.Instance, nextAnim);
                animation.SetAnimationListener(pageAnimationListener);

                return animation;
            }
            else
            {
                return base.OnCreateAnimation(transit, enter, nextAnim);
            }
        }

        public virtual void OnPageEnterAnimationCompleted()
        {
            IsPageEnterAnimationCompleted = true;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            loggingService.Log($"{GetType().Name}.OnSaveInstanceState() (from base)");

            base.OnSaveInstanceState(outState);
        }

        #region Lifecycle Events
        public override void OnHiddenChanged(bool hidden)
        {
            loggingService.Log($"{GetType().Name}.OnHiddenChanged(hidden={hidden}) (from base)");

            base.OnHiddenChanged(hidden);

            if (hidden)
            {
                ViewModel?.OnDisappearing();
                OnViewDisappearing();
            }
            else
            {
                ViewModel?.OnAppearing();
                OnViewAppearing();
            }
        }

        public override void OnStart()
        {
            loggingService.Log($"{GetType().Name}.OnStart() (from base)");

            isVisible = true;

            if (IsCurrentPage() && isVisible)
            {
                ViewModel?.OnAppearing();
                OnViewAppearing();
            }

            base.OnStart();
        }

        public override async void OnResume()
        {
            loggingService.Log($"{GetType().Name}.OnResume() (from base)");

            base.OnResume();

            isVisible = true;

            if (onApprearedSent)//we want to send OnAppeared only once
                return;

            if (IsCurrentPage() && isVisible)
            {
                onApprearedSent = true;
                await Task.Delay(600);
                ViewModel?.OnAppeared();
                OnViewAppeared();
            }
        }

        public override void OnPause()
        {
            loggingService.Log($"{GetType().Name}.OnPause() (from base)");

            isVisible = false;

            base.OnPause();
        }

        public override void OnStop()
        {
            loggingService.Log($"{GetType().Name}.OnStop() (from base)");

            isVisible = false;
            if (IsCurrentPage())
            {
                ViewModel?.OnDisappearing();
                OnViewDisappearing();
            }

            base.OnStop();
        }

        protected virtual void OnViewAppeared()
        {

        }

        protected virtual void OnViewAppearing()
        {

        }

        protected virtual void OnViewDisappearing()
        {

        }

        public override void OnDestroyView()
        {
            loggingService.Log($"{GetType().Name}.OnDestroyView() (from base)");

            base.OnDestroyView();

            //unsubscribe from SetOnApplyWindowInsetsListener
            if (Build.VERSION.SdkInt >= BuildVersionCodes.VanillaIceCream)
            {
                // Apply padding dynamically to your root layout            
                ViewCompat.SetOnApplyWindowInsetsListener(rootLayout, null);
            }
        }

        public override void OnDestroy()
        {
            loggingService.Log($"{GetType().Name}.OnDestroy() (from base)");

            base.OnDestroy();

            if (pageAnimationListener != null)
            {
                pageAnimationListener.Dispose();
                pageAnimationListener = null;
            }

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel?.Destroy();
        }
        #endregion

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            loggingService.Log($"{GetType().Name}.ViewModel_PropertyChanged({e.PropertyName})");

            //if (e.PropertyName == BusyLoadingProp)
            //{
            //    if (this.ViewModel.BusyLoading)
            //    {
            //        this.ShowLoadingIndicator(this.ViewModel.LoadingText);
            //    }
            //    else
            //    {
            //        this.HideLoadingIndicator();
            //    }
            //}
            //else if (e.PropertyName == ToastSeverityProp)
            //{
            //    severity = this.ViewModel.ToastSeverity;
            //}
            //else if (e.PropertyName == ToastMessageProp)
            //{
            //    if (!string.IsNullOrEmpty(this.ViewModel.ToastMessage))
            //    {
            //        MainActivity.Instance.ShowSnackBar(this.ViewModel.ToastMessage, severity);
            //    }
            //}
            //else
            //{
                OnViewModelPropertyChanged(e.PropertyName);
            //}
        }

        protected virtual void OnViewModelPropertyChanged(string propertyName)
        {

        }

        private async void BtnBack_Click(object sender, EventArgs e)
        {
            await ViewModel.BackCommand.ExecuteAsync();
        }

        //public void ShowLoadingIndicator(string msg)
        //{
        //    this.txtLoadingMsg.Text = msg;
        //    this.loadingView.Visibility = ViewStates.Visible;
        //    this.loadingView.FadeTo(300, 0, 1, null);
        //}

        //public void HideLoadingIndicator()
        //{
        //    this.loadingView.FadeTo(300, 1, 0, () =>
        //    {
        //        this.loadingView.Visibility = ViewStates.Invisible;
        //    });
        //}

        private bool IsCurrentPage()
        {
            var currentVisiblePage = MainActivity.Instance.pageNavigationService.GetCurrentPage();

            if (this != currentVisiblePage)
            {
                return false;
            }

            return true;
        }
    }
}
