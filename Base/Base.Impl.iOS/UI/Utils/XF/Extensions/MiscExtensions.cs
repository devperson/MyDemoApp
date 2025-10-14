using Base.Abstractions.AppService;
using System.Drawing;

namespace Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
public static class ColorExtensions
{

    public static CGColor ToCGColor(this XfColor color)
    {
        return color.ToUIColor().CGColor;
    }


    public static UIColor FromPatternImageFromBundle(string bgImage)
    {
        var image = UIImage.FromBundle(bgImage);
        if (image == null)
            return UIColor.White;

        return UIColor.FromPatternImage(image);
    }


    public static XfColor ToColor(this UIColor color)
    {
        nfloat red;
        nfloat green;
        nfloat blue;
        nfloat alpha;

        color.GetRGBA(out red, out green, out blue, out alpha);

        return new XfColor(red, green, blue, alpha);
    }



    public static UIColor ToUIColor(this XfColor color)
    {
        return new UIColor((float)color.R, (float)color.G, (float)color.B, (float)color.A);
    }

    public static UIColor ToUIColor(this XfColor color, XfColor defaultColor)
    {
        if (color.IsDefault)
            return defaultColor.ToUIColor();

        return color.ToUIColor();
    }

    public static UIColor ToUIColor(this XfColor color, UIColor defaultColor)
    {
        if (color.IsDefault)
            return defaultColor;

        return color.ToUIColor();
    }
}

public static class SizeExtensions
{
    public static SizeF ToSizeF(this Size size)
    {
        return new SizeF((float)size.Width, (float)size.Height);
    }
}

