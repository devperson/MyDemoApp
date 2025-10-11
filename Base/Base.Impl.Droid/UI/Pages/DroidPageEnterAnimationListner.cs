using Android.Views.Animations;
using AndroidX.Annotations;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Droid.UI.Pages
{
    internal class DroidPageEnterAnimationListner : Java.Lang.Object, Animation.IAnimationListener
    {
        private readonly DroidLifecyclePage page;

        public DroidPageEnterAnimationListner(DroidLifecyclePage page)
        {
            this.page = page;
        }

        public void OnAnimationEnd(Animation animation)
        {
            Platform.CurrentActivity.RunOnUiThread(() =>
            {
                page.OnPageEnterAnimationCompleted();
            });
        }

        public void OnAnimationRepeat(Animation animation)
        {

        }

        public void OnAnimationStart(Animation animation)
        {

        }
    }
}
