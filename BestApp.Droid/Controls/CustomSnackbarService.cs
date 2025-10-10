using Android.OS;
using Android.Views;
using BestApp.Abstraction.Main.UI;
using BestApp.X.Droid.Utils;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Controls;
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

    public void Hide()
    {
        snackbarView.Post(() =>
        {
            var hideY = GetTranslateY(snackbarView);
            snackbarView.Animate()
                .TranslationY(hideY)
                .SetDuration(300)
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
}
