using Android.Views;
using Base.Abstractions.Diagnostic;
using BestApp.ViewModels.Base;
using BestApp.X.Droid.Utils;
using DryIoc;

namespace BestApp.X.Droid.Pages.Base
{
    public class BasePage : AndroidX.Fragment.App.Fragment, IDispatchEventListener
    {
        public PageViewModel ViewModel { get; set; }
        protected ILoggingService loggingService { get; set; }
        private Point downPosition;
        private DateTime downTime;

        /// <summary>
        /// Indicates is wether page was navigated with animation. 
        /// It is usefull when navigating back (pop) to check if we need to apply animation for navigation back, 
        /// so it will be consistent with forward (push) navigation. 
        /// </summary>
        internal bool pushNavAnimated;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (loggingService == null)
                loggingService = MainActivity.Instance.Container.Resolve<ILoggingService>();
        }


        /// <summary>
        /// when user click on page we should hide keyboard
        /// </summary>        
        public virtual void DispatchTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                downTime = DateTime.UtcNow;
                downPosition = new Point(e.RawX, e.RawY);
            }

            if (e.Action != MotionEventActions.Up)
                return;

            var currentView = MainActivity.Instance.CurrentFocus;

            if (!(currentView is EditText))
                return;
          
            var newCurrentView = MainActivity.Instance.CurrentFocus;

            if (currentView != newCurrentView)
                return;

            double distance = downPosition.Distance(new Point(e.RawX, e.RawY));

            if (distance > Context.ToPixels(20) || DateTime.UtcNow - downTime > TimeSpan.FromMilliseconds(200))
                return;

            var location = new int[2];
            currentView.GetLocationOnScreen(location);

            float x = e.RawX + currentView.Left - location[0];
            float y = e.RawY + currentView.Top - location[1];

            var rect = new Rectangle(currentView.Left, currentView.Top, currentView.Width, currentView.Height);

            if (rect.Contains(x, y))
                return;

            Context.HideKeyboard(currentView);
            MainActivity.Instance.Window.DecorView.ClearFocus();
        }
    }

    public interface IDispatchEventListener
    {
        void DispatchTouchEvent(MotionEvent e);
    }

}
