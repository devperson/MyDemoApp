using Base.Abstractions.AppService;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;

namespace BestApp.X.iOS.Utils;
public class UIColorConstants
{    
    public static UIColor BlueColor = XfColor.FromHex("#1989FC").ToUIColor();
    public static UIColor BlueColor2 = XfColor.FromHex("#E1F0FF").ToUIColor();
    public static UIColor LabelColor = XfColor.FromHex("#353535").ToUIColor();
    public static UIColor LinkColor = XfColor.FromHex("#567cd7").ToUIColor();

    public static UIColor Gray100 = XfColor.FromHex("#f6f7f8").ToUIColor();
    public static UIColor Gray200 = XfColor.FromHex("#ebecef").ToUIColor();
    public static UIColor Gray250 = XfColor.FromHex("#D9DCE1").ToUIColor();
    public static UIColor Gray300 = XfColor.FromHex("#ced2d9").ToUIColor();
    public static UIColor Gray400 = XfColor.FromHex("#b2b8c2").ToUIColor();
    public static UIColor Gray500 = XfColor.FromHex("#959eac").ToUIColor();
    public static UIColor Gray600 = XfColor.FromHex("#788396").ToUIColor();
    public static UIColor Gray700 = XfColor.FromHex("#606a7b").ToUIColor();
    public static UIColor Gray800 = XfColor.FromHex("#4a515e").ToUIColor();
    public static UIColor Gray900 = XfColor.FromHex("#22262e").ToUIColor();
}
