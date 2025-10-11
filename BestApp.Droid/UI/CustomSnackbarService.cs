using Android.Views;
using Base.Abstractions.UI;
using Microsoft.Maui.ApplicationModel;

namespace BestApp.X.Droid.UI;
public class CustomSnackbarService : ISnackbarService
{
    private View snackbarView;    
    private bool isVisible = false;

    public void Show(string message, int duration = 3000)
    {
        snackbarView = Platform.CurrentActivity.FindViewById(Resource.Id.custom_snackbar);
        var textView = snackbarView.FindViewById<TextView>(Resource.Id.snackbar_text);
        textView.Text = message;
        // Make sure it's measured
        snackbarView.Post(() =>
        {
            snackbarView.TranslationY = -snackbarView.Height; // hide it initially
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

    public void Hide()
    {
        snackbarView.Post(() =>
        {
            float hideY = -snackbarView.Height;
            snackbarView.Animate()
                .TranslationY(hideY)
                .SetDuration(300)
                .Start();

            isVisible = false;
        });
    }
}
