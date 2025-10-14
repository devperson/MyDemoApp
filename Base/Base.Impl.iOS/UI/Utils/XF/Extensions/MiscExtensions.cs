using Base.Abstractions.AppService;
using System.Drawing;

namespace Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
public static class ColorExtensions
{

    public static CGColor ToCGColor(this XfColor color)
    {
        return color.ToUIColor().CGColor;
    }

#if __MOBILE__
    public static UIColor FromPatternImageFromBundle(string bgImage)
    {
        var image = UIImage.FromBundle(bgImage);
        if (image == null)
            return UIColor.White;

        return UIColor.FromPatternImage(image);
    }
#endif

    public static XfColor ToColor(this UIColor color)
    {
        nfloat red;
        nfloat green;
        nfloat blue;
        nfloat alpha;
#if __MOBILE__
        color.GetRGBA(out red, out green, out blue, out alpha);
#else
			if (color.Type == NSColorType.Catalog)
				throw new InvalidOperationException("Cannot convert a NSColorType.Catalog color without specifying the color space, use the overload to specify an NSColorSpace");

			color.GetRgba(out red, out green, out blue, out alpha);
#endif
        return new XfColor(red, green, blue, alpha);
    }

#if __MACOS__
		public static Color ToColor(this UIColor color, NSColorSpace colorSpace)
		{
			var convertedColor = color.UsingColorSpace(colorSpace);

			return convertedColor.ToColor();
		}
#endif

#if __MOBILE__
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
#else
		public static NSColor ToNSColor(this Color color)
		{
			return NSColor.FromRgba((float)color.R, (float)color.G, (float)color.B, (float)color.A);
		}

		public static NSColor ToNSColor(this Color color, Color defaultColor)
		{
			if (color.IsDefault)
				return defaultColor.ToNSColor();

			return color.ToNSColor();
		}

		public static NSColor ToNSColor(this Color color, NSColor defaultColor)
		{
			if (color.IsDefault)
				return defaultColor;

			return color.ToNSColor();
		}
#endif
}

//public static class PointExtensions
//{
//    public static Point ToPoint(this PointF point)
//    {
//        return new Point(point.X, point.Y);
//    }

//    public static PointF ToPointF(this Point point)
//    {
//        return new PointF(point.X, point.Y);
//    }
//}

public static class SizeExtensions
{
    public static SizeF ToSizeF(this Size size)
    {
        return new SizeF((float)size.Width, (float)size.Height);
    }
}

//public static class RectangleExtensions
//{
//    public static Rectangle ToRectangle(this RectangleF rect)
//    {
//        return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
//    }

//    public static RectangleF ToRectangleF(this Rectangle rect)
//    {
//        return new RectangleF((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);
//    }
//}
