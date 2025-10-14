using Base.Abstractions.AppService;
using Base.Abstractions.UI;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.UI;
public static class SnackbarColors
{
    public static XfColor GetBackgroundColor(this SeverityType severty)
    {
        if (severty == SeverityType.Info)
            return ColorConstants.InfoColor;
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return ColorConstants.ErrorColor;
        else
            return ColorConstants.SuccessColor;
    }

    public static XfColor GetTextColor(this SeverityType severty)
    {
        if (severty == SeverityType.Info)
            return ColorConstants.InfoTextColor;
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return ColorConstants.ErrorTextColor;
        else
            return ColorConstants.SuccessTextColor;
    }
}
