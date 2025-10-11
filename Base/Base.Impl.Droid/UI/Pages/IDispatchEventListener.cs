using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Droid.UI.Pages
{
    public interface IDispatchEventListener
    {
        void DispatchTouchEvent(MotionEvent e);
    }
}
