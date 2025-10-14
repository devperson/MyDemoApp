using Android.Graphics;
using Base.Abstractions.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Droid.UI.Utils;
internal static class ColorExtenstions
{
    public static Color ToAndroid(this XfColor self)
    {
        return new Color((byte)(byte.MaxValue * self.R), (byte)(byte.MaxValue * self.G), (byte)(byte.MaxValue * self.B), (byte)(byte.MaxValue * self.A));
    }
}
