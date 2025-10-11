using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Droid.UI.Utils;

public static class CommonExtensions
{
    public static JavaObjWrapper ToJavaObj(this object model)
    {
        return new JavaObjWrapper
        {
            Model = model,
        };
    }

    public static object ToModel(this Java.Lang.Object jobj)
    {
        var wrapper = jobj as JavaObjWrapper;
        return wrapper.Model;
    }

    public static ViewStates ToVisibility(this bool val, bool makeGone = false)
    {
        if (val)
        {
            return ViewStates.Visible;
        }
        else
        {
            if (makeGone)
            {
                return ViewStates.Gone;
            }
            else
            {
                return ViewStates.Invisible;
            }
        }
    }

    public static ViewStates ToNotVisibility(this bool val, bool makeGone = false)
    {
        if (val)
        {
            if (makeGone)
            {
                return ViewStates.Gone;
            }
            else
            {
                return ViewStates.Invisible;
            }

        }
        else
        {
            return ViewStates.Visible;
        }
    }
}
