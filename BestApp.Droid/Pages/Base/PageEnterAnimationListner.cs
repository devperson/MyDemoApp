using Android.Views.Animations;
using AndroidX.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Base
{
    internal class PageEnterAnimationListner : Java.Lang.Object, Animation.IAnimationListener //Animator.IAnimatorListener
    {
        private readonly LifecyclePage page;

        public PageEnterAnimationListner(LifecyclePage page)
        {
            this.page = page;
        }

        public void OnAnimationEnd(Animation animation)
        {
            MainActivity.Instance.RunOnUiThread(() =>
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
