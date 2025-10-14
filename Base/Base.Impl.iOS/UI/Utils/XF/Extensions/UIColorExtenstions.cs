using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;

public static class UIColorExtenstions
{
    public static UIColor MakeDarker(this UIColor color, float percent)
    {
        var helper = new ColorHelper(color);
        return helper.Darker(percent);
    }

    public static UIColor MakeLighter(this UIColor color, float percent)
    {
        var helper = new ColorHelper(color);
        return helper.Lighter(percent);
    }
}
