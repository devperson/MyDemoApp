using Base.Abstractions.AppService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Impl.Texture.iOS.UI.Utils.Styles;
public class ColorConstants
{
    public static XfColor PrimaryColor { get; set; } = XfColor.FromHex("#1989FC");
    public static XfColor PrimaryColor2 { get; set; } = XfColor.FromHex("#E1F0FF");
    public static XfColor BgColor { get; set; }= XfColor.FromHex("#FFF9F9FA");
    public static XfColor DefaultTextColor = XfColor.FromHex("#353535");


    public static XfColor Gray100 = XfColor.FromHex("#f6f7f8");
    public static XfColor Gray200 = XfColor.FromHex("#ebecef");
    public static XfColor Gray250 = XfColor.FromHex("#D9DCE1");
    public static XfColor Gray300 = XfColor.FromHex("#ced2d9");
    public static XfColor Gray400 = XfColor.FromHex("#b2b8c2");
    public static XfColor Gray500 = XfColor.FromHex("#959eac");
    public static XfColor Gray600 = XfColor.FromHex("#788396");
    public static XfColor Gray700 = XfColor.FromHex("#606a7b");
    public static XfColor Gray800 = XfColor.FromHex("#4a515e");
    public static XfColor Gray900 = XfColor.FromHex("#22262e");

    public static XfColor InfoColor { get; set; } = XfColor.FromHex("#E1F0FF");
    public static XfColor InfoTextColor { get; set; } = XfColor.FromHex("#FFF9F9FA");

    public static XfColor ErrorColor { get; set; } = XfColor.FromHex("#FFEAEB");
    public static XfColor ErrorTextColor { get; set; } = XfColor.FromHex("#ff4444");

    public static XfColor SuccessColor { get; set; } = XfColor.FromHex("#FFCDFFCD");
    public static XfColor SuccessTextColor { get; set; } = XfColor.FromHex("#FF114338");

}
