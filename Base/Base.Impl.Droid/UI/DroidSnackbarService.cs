using Android.Views;
using Base.Abstractions.UI;
using Base.Impl.Droid.UI.Utils;
using Base.Impl.UI;
using Microsoft.Maui.ApplicationModel;

namespace Base.Impl.Droid.UI;

public class DroidSnackbarService : ISnackbarService
{
    private ViewGroup rootView;
    private View snackbarView;    
    private bool isVisible = false;

    public void Show(string message, int duration = 3000)
    {
      
    }

    public void Hide()
    {
        snackbarView.Post(() =>
        {
            var hideY = GetTranslateY(snackbarView);
            snackbarView.Animate()
                .TranslationY(hideY)
                .SetDuration(300)
                .WithEndAction(new Java.Lang.Runnable(() =>
                {
                    rootView.RemoveView(snackbarView);
                }))
                .Start();

            isVisible = false;
        });
    }

    private float GetTranslateY(View view)
    {
        float hideY = -(view.Height + GetTopMargin(snackbarView));

        return hideY;
    }

    private float GetTopMargin(View view)
    {
        if (view.LayoutParameters is ViewGroup.MarginLayoutParams marginParams)
        {
            return marginParams.TopMargin;
        }
        return 0;
    }

    public void ShowError(string message)
    {
        this.Show(message, SeverityType.Error);
    }

    public void ShowInfo(string message)
    {
        this.Show(message, SeverityType.Info);
    }

    public void Show(string message, SeverityType severityType, int duration = 3000)
    {
        var activity = Platform.CurrentActivity;
        var inflater = LayoutInflater.From(activity);

        rootView = activity.Window.DecorView.FindViewById<ViewGroup>(Android.Resource.Id.Content);
        snackbarView = inflater.Inflate(Resource.Layout.custom_snackbar, rootView, false);
        
        if (rootView is not FrameLayout)
        {
            throw new Exception("To ensure the Snackbar works correctly, it’s recommended to use a FrameLayout as the Activity’s root view.");
        }
        snackbarView.SetBackgroundColor(severityType.GetBackgroundColor().ToAndroid());

        var textView = snackbarView.FindViewById<TextView>(Resource.Id.snackbar_text);
        textView.Text = message;
        textView.SetTextColor(severityType.GetTextColor().ToAndroid());

        rootView.AddView(snackbarView);

        // Make sure it's measured
        snackbarView.Post(() =>
        {
            snackbarView.TranslationY = GetTranslateY(snackbarView); // hide it initially
        });

        snackbarView.Post(() =>
        {
            snackbarView.Animate()
                .TranslationY(0)
                .SetDuration(300)
                .Start();

            isVisible = true;

            // Auto hide after duration
            snackbarView.PostDelayed(() =>
            {
                if (isVisible)
                    Hide();
            }, duration);
        });
    }
}
