using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Utils;

public class JavaObjWrapper : Java.Lang.Object
{
    public JavaObjWrapper()
    {
    }
    public JavaObjWrapper(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
    {
    }

    public object Model;
}
