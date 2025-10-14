using Base.Abstractions.AppService;
using Base.Abstractions.UI;
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
            return XfColor.FromHex("#E1F0FF");
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return XfColor.FromHex("#FFEAEB");
        else
            return XfColor.FromHex("#FFCDFFCD");
    }

    public static XfColor GetTextColor(this SeverityType severty)
    {
        if (severty == SeverityType.Info)
            return XfColor.FromHex("#FFF9F9FA");
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return XfColor.FromHex("#ff4444");
        else
            return XfColor.FromHex("#FF114338");
    }
}
