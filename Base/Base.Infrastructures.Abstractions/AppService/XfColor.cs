using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Base.Abstractions.AppService;
public struct XfColor
{
    readonly Mode _mode;

    enum Mode
    {
        Default,
        Rgb,
        Hsl
    }

    public static XfColor Default
    {
        get { return new XfColor(-1d, -1d, -1d, -1d, Mode.Default); }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool IsDefault
    {
        get { return _mode == Mode.Default; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetAccent(XfColor value) => Accent = value;
    public static XfColor Accent { get; internal set; }

    readonly float _a;

    public double A
    {
        get { return _a; }
    }

    readonly float _r;

    public double R
    {
        get { return _r; }
    }

    readonly float _g;

    public double G
    {
        get { return _g; }
    }

    readonly float _b;

    public double B
    {
        get { return _b; }
    }

    readonly float _hue;

    public double Hue
    {
        get { return _hue; }
    }

    readonly float _saturation;

    public double Saturation
    {
        get { return _saturation; }
    }

    readonly float _luminosity;

    public double Luminosity
    {
        get { return _luminosity; }
    }

    public XfColor(double r, double g, double b, double a) : this(r, g, b, a, Mode.Rgb)
    {
    }

    XfColor(double w, double x, double y, double z, Mode mode)
    {
        _mode = mode;
        switch (mode)
        {
            default:
            case Mode.Default:
                _r = _g = _b = _a = -1;
                _hue = _saturation = _luminosity = -1;
                break;
            case Mode.Rgb:
                _r = (float)Clamp(w, 0, 1);
                _g = (float)Clamp(x, 0, 1);
                _b = (float)Clamp(y, 0, 1);
                _a = (float)Clamp(z, 0, 1);
                ConvertToHsl(_r, _g, _b, mode, out _hue, out _saturation, out _luminosity);
                break;
            case Mode.Hsl:
                _hue = (float)Clamp(w, 0, 1);
                _saturation = (float)Clamp(x, 0, 1);
                _luminosity = (float)Clamp(y, 0, 1);
                _a = (float)Clamp(z, 0, 1);
                ConvertToRgb(_hue, _saturation, _luminosity, mode, out _r, out _g, out _b);
                break;
        }
    }

    XfColor(int r, int g, int b)
    {
        _mode = Mode.Rgb;
        _r = r / 255f;
        _g = g / 255f;
        _b = b / 255f;
        _a = 1;
        ConvertToHsl(_r, _g, _b, _mode, out _hue, out _saturation, out _luminosity);
    }

    XfColor(int r, int g, int b, int a)
    {
        _mode = Mode.Rgb;
        _r = r / 255f;
        _g = g / 255f;
        _b = b / 255f;
        _a = a / 255f;
        ConvertToHsl(_r, _g, _b, _mode, out _hue, out _saturation, out _luminosity);
    }

    public XfColor(double r, double g, double b) : this(r, g, b, 1)
    {
    }

    public XfColor(double value) : this(value, value, value, 1)
    {
    }

    public XfColor MultiplyAlpha(double alpha)
    {
        switch (_mode)
        {
            default:
            case Mode.Default:
                throw new InvalidOperationException("Invalid on Color.Default");
            case Mode.Rgb:
                return new XfColor(_r, _g, _b, _a * alpha, Mode.Rgb);
            case Mode.Hsl:
                return new XfColor(_hue, _saturation, _luminosity, _a * alpha, Mode.Hsl);
        }
    }

    public XfColor AddLuminosity(double delta)
    {
        if (_mode == Mode.Default)
            throw new InvalidOperationException("Invalid on Color.Default");

        return new XfColor(_hue, _saturation, _luminosity + delta, _a, Mode.Hsl);
    }

    public XfColor WithHue(double hue)
    {
        if (_mode == Mode.Default)
            throw new InvalidOperationException("Invalid on Color.Default");
        return new XfColor(hue, _saturation, _luminosity, _a, Mode.Hsl);
    }

    public XfColor WithSaturation(double saturation)
    {
        if (_mode == Mode.Default)
            throw new InvalidOperationException("Invalid on Color.Default");
        return new XfColor(_hue, saturation, _luminosity, _a, Mode.Hsl);
    }

    public XfColor WithLuminosity(double luminosity)
    {
        if (_mode == Mode.Default)
            throw new InvalidOperationException("Invalid on Color.Default");
        return new XfColor(_hue, _saturation, luminosity, _a, Mode.Hsl);
    }

    static void ConvertToRgb(float hue, float saturation, float luminosity, Mode mode, out float r, out float g, out float b)
    {
        if (mode != Mode.Hsl)
            throw new InvalidOperationException();

        if (luminosity == 0)
        {
            r = g = b = 0;
            return;
        }

        if (saturation == 0)
        {
            r = g = b = luminosity;
            return;
        }
        float temp2 = luminosity <= 0.5f ? luminosity * (1.0f + saturation) : luminosity + saturation - luminosity * saturation;
        float temp1 = 2.0f * luminosity - temp2;

        var t3 = new[] { hue + 1.0f / 3.0f, hue, hue - 1.0f / 3.0f };
        var clr = new float[] { 0, 0, 0 };
        for (var i = 0; i < 3; i++)
        {
            if (t3[i] < 0)
                t3[i] += 1.0f;
            if (t3[i] > 1)
                t3[i] -= 1.0f;
            if (6.0 * t3[i] < 1.0)
                clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
            else if (2.0 * t3[i] < 1.0)
                clr[i] = temp2;
            else if (3.0 * t3[i] < 2.0)
                clr[i] = temp1 + (temp2 - temp1) * (2.0f / 3.0f - t3[i]) * 6.0f;
            else
                clr[i] = temp1;
        }

        r = clr[0];
        g = clr[1];
        b = clr[2];
    }

    static void ConvertToHsl(float r, float g, float b, Mode mode, out float h, out float s, out float l)
    {
        float v = Math.Max(r, g);
        v = Math.Max(v, b);

        float m = Math.Min(r, g);
        m = Math.Min(m, b);

        l = (m + v) / 2.0f;
        if (l <= 0.0)
        {
            h = s = l = 0;
            return;
        }
        float vm = v - m;
        s = vm;

        if (s > 0.0)
        {
            s /= l <= 0.5f ? v + m : 2.0f - v - m;
        }
        else
        {
            h = 0;
            s = 0;
            return;
        }

        float r2 = (v - r) / vm;
        float g2 = (v - g) / vm;
        float b2 = (v - b) / vm;

        if (r == v)
        {
            h = g == m ? 5.0f + b2 : 1.0f - g2;
        }
        else if (g == v)
        {
            h = b == m ? 1.0f + r2 : 3.0f - b2;
        }
        else
        {
            h = r == m ? 3.0f + g2 : 5.0f - r2;
        }
        h /= 6.0f;
    }

    public static bool operator ==(XfColor color1, XfColor color2)
    {
        return EqualsInner(color1, color2);
    }

    public static bool operator !=(XfColor color1, XfColor color2)
    {
        return !EqualsInner(color1, color2);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = _r.GetHashCode();
            hashcode = (hashcode * 397) ^ _g.GetHashCode();
            hashcode = (hashcode * 397) ^ _b.GetHashCode();
            hashcode = (hashcode * 397) ^ _a.GetHashCode();
            return hashcode;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is XfColor)
        {
            return EqualsInner(this, (XfColor)obj);
        }
        return base.Equals(obj);
    }

    static bool EqualsInner(XfColor color1, XfColor color2)
    {
        if (color1._mode == Mode.Default && color2._mode == Mode.Default)
            return true;
        if (color1._mode == Mode.Default || color2._mode == Mode.Default)
            return false;
        if (color1._mode == Mode.Hsl && color2._mode == Mode.Hsl)
            return color1._hue == color2._hue && color1._saturation == color2._saturation && color1._luminosity == color2._luminosity && color1._a == color2._a;
        return color1._r == color2._r && color1._g == color2._g && color1._b == color2._b && color1._a == color2._a;
    }

    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[Color: A={0}, R={1}, G={2}, B={3}, Hue={4}, Saturation={5}, Luminosity={6}]", A, R, G, B, Hue, Saturation, Luminosity);
    }

    public string ToHex()
    {
        var red = (uint)(R * 255);
        var green = (uint)(G * 255);
        var blue = (uint)(B * 255);
        var alpha = (uint)(A * 255);
        return $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";
    }

    static uint ToHex(char c)
    {
        ushort x = (ushort)c;
        if (x >= '0' && x <= '9')
            return (uint)(x - '0');

        x |= 0x20;
        if (x >= 'a' && x <= 'f')
            return (uint)(x - 'a' + 10);
        return 0;
    }

    static uint ToHexD(char c)
    {
        var j = ToHex(c);
        return (j << 4) | j;
    }

    public static XfColor FromHex(string hex)
    {
        // Undefined
        if (hex.Length < 3)
            return Default;
        int idx = (hex[0] == '#') ? 1 : 0;

        switch (hex.Length - idx)
        {
            case 3: //#rgb => ffrrggbb
                var t1 = ToHexD(hex[idx++]);
                var t2 = ToHexD(hex[idx++]);
                var t3 = ToHexD(hex[idx]);

                return FromRgb((int)t1, (int)t2, (int)t3);

            case 4: //#argb => aarrggbb
                var f1 = ToHexD(hex[idx++]);
                var f2 = ToHexD(hex[idx++]);
                var f3 = ToHexD(hex[idx++]);
                var f4 = ToHexD(hex[idx]);
                return FromRgba((int)f2, (int)f3, (int)f4, (int)f1);

            case 6: //#rrggbb => ffrrggbb
                return FromRgb((int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])));

            case 8: //#aarrggbb
                var a1 = ToHex(hex[idx++]) << 4 | ToHex(hex[idx++]);
                return FromRgba((int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx++])),
                        (int)(ToHex(hex[idx++]) << 4 | ToHex(hex[idx])),
                        (int)a1);

            default: //everything else will result in unexpected results
                return Default;
        }
    }

    public static XfColor FromUint(uint argb)
    {
        return FromRgba((byte)((argb & 0x00ff0000) >> 0x10), (byte)((argb & 0x0000ff00) >> 0x8), (byte)(argb & 0x000000ff), (byte)((argb & 0xff000000) >> 0x18));
    }

    public static XfColor FromRgba(int r, int g, int b, int a)
    {
        double red = (double)r / 255;
        double green = (double)g / 255;
        double blue = (double)b / 255;
        double alpha = (double)a / 255;
        return new XfColor(red, green, blue, alpha, Mode.Rgb);
    }

    public static XfColor FromRgb(int r, int g, int b)
    {
        return FromRgba(r, g, b, 255);
    }

    public static XfColor FromRgba(double r, double g, double b, double a)
    {
        return new XfColor(r, g, b, a);
    }

    public static XfColor FromRgb(double r, double g, double b)
    {
        return new XfColor(r, g, b, 1d, Mode.Rgb);
    }

    public static XfColor FromHsla(double h, double s, double l, double a = 1d)
    {
        return new XfColor(h, s, l, a, Mode.Hsl);
    }

    public static XfColor FromHsva(double h, double s, double v, double a)
    {
        h = Clamp(h, 0, 1);
        s = Clamp(s, 0, 1);
        v = Clamp(v, 0, 1);
        var range = (int)(Math.Floor(h * 6)) % 6;
        var f = h * 6 - Math.Floor(h * 6);
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        switch (range)
        {
            case 0:
                return FromRgba(v, t, p, a);
            case 1:
                return FromRgba(q, v, p, a);
            case 2:
                return FromRgba(p, v, t, a);
            case 3:
                return FromRgba(p, q, v, a);
            case 4:
                return FromRgba(t, p, v, a);
        }
        return FromRgba(v, p, q, a);
    }

    public static XfColor FromHsv(double h, double s, double v)
    {
        return FromHsva(h, s, v, 1d);
    }

    public static XfColor FromHsva(int h, int s, int v, int a)
    {
        return FromHsva(h / 360d, s / 100d, v / 100d, a / 100d);
    }

    public static XfColor FromHsv(int h, int s, int v)
    {
        return FromHsva(h / 360d, s / 100d, v / 100d, 1d);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double self, double min, double max)
    {
        if (max < min)
        {
            return max;
        }
        else if (self < min)
        {
            return min;
        }
        else if (self > max)
        {
            return max;
        }

        return self;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int self, int min, int max)
    {
        if (max < min)
        {
            return max;
        }
        else if (self < min)
        {
            return min;
        }
        else if (self > max)
        {
            return max;
        }

        return self;
    }

#if !NETSTANDARD1_0
    public static implicit operator System.Drawing.Color(XfColor color)
    {
        if (color.IsDefault)
            return System.Drawing.Color.Empty;
        return System.Drawing.Color.FromArgb((byte)(color._a * 255), (byte)(color._r * 255), (byte)(color._g * 255), (byte)(color._b * 255));
    }

    public static implicit operator XfColor(System.Drawing.Color color)
    {
        if (color.IsEmpty)
            return XfColor.Default;
        return FromRgba(color.R, color.G, color.B, color.A);
    }
#endif
    #region Color Definitions

    // matches colors in WPF's System.Windows.Media.Colors
    public static readonly XfColor AliceBlue = new XfColor(240, 248, 255);
    public static readonly XfColor AntiqueWhite = new XfColor(250, 235, 215);
    public static readonly XfColor Aqua = new XfColor(0, 255, 255);
    public static readonly XfColor Aquamarine = new XfColor(127, 255, 212);
    public static readonly XfColor Azure = new XfColor(240, 255, 255);
    public static readonly XfColor Beige = new XfColor(245, 245, 220);
    public static readonly XfColor Bisque = new XfColor(255, 228, 196);
    public static readonly XfColor Black = new XfColor(0, 0, 0);
    public static readonly XfColor BlanchedAlmond = new XfColor(255, 235, 205);
    public static readonly XfColor Blue = new XfColor(0, 0, 255);
    public static readonly XfColor BlueViolet = new XfColor(138, 43, 226);
    public static readonly XfColor Brown = new XfColor(165, 42, 42);
    public static readonly XfColor BurlyWood = new XfColor(222, 184, 135);
    public static readonly XfColor CadetBlue = new XfColor(95, 158, 160);
    public static readonly XfColor Chartreuse = new XfColor(127, 255, 0);
    public static readonly XfColor Chocolate = new XfColor(210, 105, 30);
    public static readonly XfColor Coral = new XfColor(255, 127, 80);
    public static readonly XfColor CornflowerBlue = new XfColor(100, 149, 237);
    public static readonly XfColor Cornsilk = new XfColor(255, 248, 220);
    public static readonly XfColor Crimson = new XfColor(220, 20, 60);
    public static readonly XfColor Cyan = new XfColor(0, 255, 255);
    public static readonly XfColor DarkBlue = new XfColor(0, 0, 139);
    public static readonly XfColor DarkCyan = new XfColor(0, 139, 139);
    public static readonly XfColor DarkGoldenrod = new XfColor(184, 134, 11);
    public static readonly XfColor DarkGray = new XfColor(169, 169, 169);
    public static readonly XfColor DarkGreen = new XfColor(0, 100, 0);
    public static readonly XfColor DarkKhaki = new XfColor(189, 183, 107);
    public static readonly XfColor DarkMagenta = new XfColor(139, 0, 139);
    public static readonly XfColor DarkOliveGreen = new XfColor(85, 107, 47);
    public static readonly XfColor DarkOrange = new XfColor(255, 140, 0);
    public static readonly XfColor DarkOrchid = new XfColor(153, 50, 204);
    public static readonly XfColor DarkRed = new XfColor(139, 0, 0);
    public static readonly XfColor DarkSalmon = new XfColor(233, 150, 122);
    public static readonly XfColor DarkSeaGreen = new XfColor(143, 188, 143);
    public static readonly XfColor DarkSlateBlue = new XfColor(72, 61, 139);
    public static readonly XfColor DarkSlateGray = new XfColor(47, 79, 79);
    public static readonly XfColor DarkTurquoise = new XfColor(0, 206, 209);
    public static readonly XfColor DarkViolet = new XfColor(148, 0, 211);
    public static readonly XfColor DeepPink = new XfColor(255, 20, 147);
    public static readonly XfColor DeepSkyBlue = new XfColor(0, 191, 255);
    public static readonly XfColor DimGray = new XfColor(105, 105, 105);
    public static readonly XfColor DodgerBlue = new XfColor(30, 144, 255);
    public static readonly XfColor Firebrick = new XfColor(178, 34, 34);
    public static readonly XfColor FloralWhite = new XfColor(255, 250, 240);
    public static readonly XfColor ForestGreen = new XfColor(34, 139, 34);
    public static readonly XfColor Fuchsia = new XfColor(255, 0, 255);
    [Obsolete("Fuschia is obsolete as of version 1.3.0. Please use Fuchsia instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly XfColor Fuschia = new XfColor(255, 0, 255);
    public static readonly XfColor Gainsboro = new XfColor(220, 220, 220);
    public static readonly XfColor GhostWhite = new XfColor(248, 248, 255);
    public static readonly XfColor Gold = new XfColor(255, 215, 0);
    public static readonly XfColor Goldenrod = new XfColor(218, 165, 32);
    public static readonly XfColor Gray = new XfColor(128, 128, 128);
    public static readonly XfColor Green = new XfColor(0, 128, 0);
    public static readonly XfColor GreenYellow = new XfColor(173, 255, 47);
    public static readonly XfColor Honeydew = new XfColor(240, 255, 240);
    public static readonly XfColor HotPink = new XfColor(255, 105, 180);
    public static readonly XfColor IndianRed = new XfColor(205, 92, 92);
    public static readonly XfColor Indigo = new XfColor(75, 0, 130);
    public static readonly XfColor Ivory = new XfColor(255, 255, 240);
    public static readonly XfColor Khaki = new XfColor(240, 230, 140);
    public static readonly XfColor Lavender = new XfColor(230, 230, 250);
    public static readonly XfColor LavenderBlush = new XfColor(255, 240, 245);
    public static readonly XfColor LawnGreen = new XfColor(124, 252, 0);
    public static readonly XfColor LemonChiffon = new XfColor(255, 250, 205);
    public static readonly XfColor LightBlue = new XfColor(173, 216, 230);
    public static readonly XfColor LightCoral = new XfColor(240, 128, 128);
    public static readonly XfColor LightCyan = new XfColor(224, 255, 255);
    public static readonly XfColor LightGoldenrodYellow = new XfColor(250, 250, 210);
    public static readonly XfColor LightGray = new XfColor(211, 211, 211);
    public static readonly XfColor LightGreen = new XfColor(144, 238, 144);
    public static readonly XfColor LightPink = new XfColor(255, 182, 193);
    public static readonly XfColor LightSalmon = new XfColor(255, 160, 122);
    public static readonly XfColor LightSeaGreen = new XfColor(32, 178, 170);
    public static readonly XfColor LightSkyBlue = new XfColor(135, 206, 250);
    public static readonly XfColor LightSlateGray = new XfColor(119, 136, 153);
    public static readonly XfColor LightSteelBlue = new XfColor(176, 196, 222);
    public static readonly XfColor LightYellow = new XfColor(255, 255, 224);
    public static readonly XfColor Lime = new XfColor(0, 255, 0);
    public static readonly XfColor LimeGreen = new XfColor(50, 205, 50);
    public static readonly XfColor Linen = new XfColor(250, 240, 230);
    public static readonly XfColor Magenta = new XfColor(255, 0, 255);
    public static readonly XfColor Maroon = new XfColor(128, 0, 0);
    public static readonly XfColor MediumAquamarine = new XfColor(102, 205, 170);
    public static readonly XfColor MediumBlue = new XfColor(0, 0, 205);
    public static readonly XfColor MediumOrchid = new XfColor(186, 85, 211);
    public static readonly XfColor MediumPurple = new XfColor(147, 112, 219);
    public static readonly XfColor MediumSeaGreen = new XfColor(60, 179, 113);
    public static readonly XfColor MediumSlateBlue = new XfColor(123, 104, 238);
    public static readonly XfColor MediumSpringGreen = new XfColor(0, 250, 154);
    public static readonly XfColor MediumTurquoise = new XfColor(72, 209, 204);
    public static readonly XfColor MediumVioletRed = new XfColor(199, 21, 133);
    public static readonly XfColor MidnightBlue = new XfColor(25, 25, 112);
    public static readonly XfColor MintCream = new XfColor(245, 255, 250);
    public static readonly XfColor MistyRose = new XfColor(255, 228, 225);
    public static readonly XfColor Moccasin = new XfColor(255, 228, 181);
    public static readonly XfColor NavajoWhite = new XfColor(255, 222, 173);
    public static readonly XfColor Navy = new XfColor(0, 0, 128);
    public static readonly XfColor OldLace = new XfColor(253, 245, 230);
    public static readonly XfColor Olive = new XfColor(128, 128, 0);
    public static readonly XfColor OliveDrab = new XfColor(107, 142, 35);
    public static readonly XfColor Orange = new XfColor(255, 165, 0);
    public static readonly XfColor OrangeRed = new XfColor(255, 69, 0);
    public static readonly XfColor Orchid = new XfColor(218, 112, 214);
    public static readonly XfColor PaleGoldenrod = new XfColor(238, 232, 170);
    public static readonly XfColor PaleGreen = new XfColor(152, 251, 152);
    public static readonly XfColor PaleTurquoise = new XfColor(175, 238, 238);
    public static readonly XfColor PaleVioletRed = new XfColor(219, 112, 147);
    public static readonly XfColor PapayaWhip = new XfColor(255, 239, 213);
    public static readonly XfColor PeachPuff = new XfColor(255, 218, 185);
    public static readonly XfColor Peru = new XfColor(205, 133, 63);
    public static readonly XfColor Pink = new XfColor(255, 192, 203);
    public static readonly XfColor Plum = new XfColor(221, 160, 221);
    public static readonly XfColor PowderBlue = new XfColor(176, 224, 230);
    public static readonly XfColor Purple = new XfColor(128, 0, 128);
    public static readonly XfColor Red = new XfColor(255, 0, 0);
    public static readonly XfColor RosyBrown = new XfColor(188, 143, 143);
    public static readonly XfColor RoyalBlue = new XfColor(65, 105, 225);
    public static readonly XfColor SaddleBrown = new XfColor(139, 69, 19);
    public static readonly XfColor Salmon = new XfColor(250, 128, 114);
    public static readonly XfColor SandyBrown = new XfColor(244, 164, 96);
    public static readonly XfColor SeaGreen = new XfColor(46, 139, 87);
    public static readonly XfColor SeaShell = new XfColor(255, 245, 238);
    public static readonly XfColor Sienna = new XfColor(160, 82, 45);
    public static readonly XfColor Silver = new XfColor(192, 192, 192);
    public static readonly XfColor SkyBlue = new XfColor(135, 206, 235);
    public static readonly XfColor SlateBlue = new XfColor(106, 90, 205);
    public static readonly XfColor SlateGray = new XfColor(112, 128, 144);
    public static readonly XfColor Snow = new XfColor(255, 250, 250);
    public static readonly XfColor SpringGreen = new XfColor(0, 255, 127);
    public static readonly XfColor SteelBlue = new XfColor(70, 130, 180);
    public static readonly XfColor Tan = new XfColor(210, 180, 140);
    public static readonly XfColor Teal = new XfColor(0, 128, 128);
    public static readonly XfColor Thistle = new XfColor(216, 191, 216);
    public static readonly XfColor Tomato = new XfColor(255, 99, 71);
    public static readonly XfColor Transparent = new XfColor(255, 255, 255, 0);
    public static readonly XfColor Turquoise = new XfColor(64, 224, 208);
    public static readonly XfColor Violet = new XfColor(238, 130, 238);
    public static readonly XfColor Wheat = new XfColor(245, 222, 179);
    public static readonly XfColor White = new XfColor(255, 255, 255);
    public static readonly XfColor WhiteSmoke = new XfColor(245, 245, 245);
    public static readonly XfColor Yellow = new XfColor(255, 255, 0);
    public static readonly XfColor YellowGreen = new XfColor(154, 205, 50);

    #endregion
}
