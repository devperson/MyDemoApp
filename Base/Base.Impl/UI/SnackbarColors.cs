using Base.Abstractions.AppService;
using Base.Abstractions.UI;

namespace Base.Impl.UI;
public static class SnackbarColors
{
    public static XfColor InfoColor { get; set; } = XfColor.FromHex("#E1F0FF");
    public static XfColor InfoTextColor { get; set; } = XfColor.FromHex("#FFF9F9FA");

    public static XfColor ErrorColor { get; set; } = XfColor.FromHex("#FFEAEB");
    public static XfColor ErrorTextColor { get; set; } = XfColor.FromHex("#ff4444");

    public static XfColor SuccessColor { get; set; } = XfColor.FromHex("#FFCDFFCD");
    public static XfColor SuccessTextColor { get; set; } = XfColor.FromHex("#FF114338");

    public static XfColor GetBackgroundColor(this SeverityType severty)
    {
        if (severty == SeverityType.Info)
            return InfoColor;
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return ErrorColor;
        else
            return SuccessColor;
    }

    public static XfColor GetTextColor(this SeverityType severty)
    {
        if (severty == SeverityType.Info)
            return InfoTextColor;
        else if (severty == SeverityType.Error || severty == SeverityType.Warning)
            return ErrorTextColor;
        else
            return SuccessTextColor;
    }
}
