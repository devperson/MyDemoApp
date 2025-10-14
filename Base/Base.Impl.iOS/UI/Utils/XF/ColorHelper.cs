
namespace Base.Impl.Texture.iOS.UI.Utils.XF
{
    public class ColorHelper
    {
        #region CONSTANTS
        const int SHADOW_ADJ = -333;
        const int HIGHLIGHT_ADJ = 500;
        const int WATERMARK_ADJ = -50;
        const int RANGE = 240;
        const int HLS_MAX = RANGE;
        const int RGB_MAX = 255;
        const int UNDEFINED = HLS_MAX * 2 / 3;
        #endregion

        #region PROPERTIES
        public int Luminosity
        {
            get
            {
                return m_Luminosity;
            }
        }
        #endregion


        #region INTERNAL VARIABLES
        int m_Hue;
        int m_Saturation;
        int m_Luminosity;
        #endregion

        #region CONSTRUCTORS
        /// <include file='doc\ControlPaint.uex' path='docs/doc[@for="ControlPaint.HLSColor.HLSColor"]/*' />
        /// <devdoc>
        /// </devdoc>
#if __IOS__
        public ColorHelper(UIColor color)
        {
            nfloat rf, gf, bf, af;
            color.GetRGBA(out rf, out gf, out bf, out af);
            int r = (int)(rf * 255), g = (int)(gf * 255), b = (int)(bf * 255);
#else
        public ColorHelper(Android.Graphics.Color color)

        {
            int r = color.R;
            int g = color.G;
            int b = color.B;
#endif
            int max, min;        /* max and min RGB values */
            int sum, dif;
            int rDelta, gDelta, bDelta;  /* intermediate value: % of spread from max */

            /* calculate lightness */
            max = Math.Max(Math.Max(r, g), b);
            min = Math.Min(Math.Min(r, g), b);
            sum = max + min;

            m_Luminosity = (((sum * HLS_MAX) + RGB_MAX) / (2 * RGB_MAX));

            dif = max - min;
            if (dif == 0)
            {       /* r=g=b --> achromatic case */
                m_Saturation = 0;                         /* saturation */
                m_Hue = UNDEFINED;                 /* hue */
            }
            else
            {                           /* chromatic case */
                                        /* saturation */
                if (m_Luminosity <= (HLS_MAX / 2))
                {
                    m_Saturation = (int)(((dif * (int)HLS_MAX) + (sum / 2)) / sum);
                }
                else
                {
                    m_Saturation = (int)((int)((dif * (int)HLS_MAX) + (int)((2 * RGB_MAX - sum) / 2))
                                        / (2 * RGB_MAX - sum));
                }
                /* hue */
                rDelta = (int)((((max - r) * (int)(HLS_MAX / 6)) + (dif / 2)) / dif);
                gDelta = (int)((((max - g) * (int)(HLS_MAX / 6)) + (dif / 2)) / dif);
                bDelta = (int)((((max - b) * (int)(HLS_MAX / 6)) + (dif / 2)) / dif);

                if ((int)r == max)
                {
                    m_Hue = bDelta - gDelta;
                }
                else if ((int)g == max)
                {
                    m_Hue = (HLS_MAX / 3) + rDelta - bDelta;
                }
                else /* B == cMax */
                {
                    m_Hue = ((2 * HLS_MAX) / 3) + gDelta - rDelta;
                }

                if (m_Hue < 0)
                {
                    m_Hue += HLS_MAX;
                }
                if (m_Hue > HLS_MAX)
                {
                    m_Hue -= HLS_MAX;
                }
            }
        }
        #endregion


        #region PUBLIC API
#if __IOS__
        public UIColor Darker(float percDarker)
#else
        /// <summary>
        /// Makes the current color darker
        /// </summary>
        public Android.Graphics.Color Darker(float percDarker)
#endif

        {
            int oneLum = 0;
            int zeroLum = NewLuma(SHADOW_ADJ, true);


            return ColorFromHLS(m_Hue, zeroLum - (int)((zeroLum - oneLum) * percDarker), m_Saturation);
        }


#if __IOS__
        public UIColor Lighter(float percLighter)
#else
        /// <summary>
        /// Makes current color lighter
        /// </summary>
        public Android.Graphics.Color Lighter(float percLighter)
#endif        
        {
            int zeroLum = m_Luminosity;
            int oneLum = NewLuma(HIGHLIGHT_ADJ, true);

            return ColorFromHLS(m_Hue, zeroLum + (int)((oneLum - zeroLum) * percLighter), m_Saturation);
        }
        #endregion


        #region HELPER FUNCTIONS
        int NewLuma(int n, bool scale)
        {
            return NewLuma(m_Luminosity, n, scale);
        }

        int NewLuma(int luminosity, int n, bool scale)
        {
            if (n == 0)
            {
                return luminosity;
            }

            if (scale)
            {
                if (n > 0)
                {
                    return (int)(((int)luminosity * (1000 - n) + (RANGE + 1L) * n) / 1000);
                }
                else
                {
                    return (int)(((int)luminosity * (n + 1000)) / 1000);
                }
            }

            int newLum = luminosity;
            newLum += (int)((long)n * RANGE / 1000);

            if (newLum < 0)
            {
                newLum = 0;
            }
            if (newLum > HLS_MAX)
            {
                newLum = HLS_MAX;
            }

            return newLum;
        }


#if __IOS__
        private UIColor ColorFromHLS(int hue, int luminosity, int saturation)
#else
        /// <summary>
        /// Creates Android.Graphics.Color from hue, luminosity, saturation
        /// </summary>
        /// <returns></returns>
        Android.Graphics.Color ColorFromHLS(int hue, int luminosity, int saturation)
#endif            
        {
            byte r, g, b;                      /* RGB component values */
            int magic1, magic2;       /* calculated magic numbers (really!) */

            if (saturation == 0)
            {                /* achromatic case */
                r = g = b = (byte)((luminosity * RGB_MAX) / HLS_MAX);
                if (hue != UNDEFINED)
                {
                    /* ERROR */
                }
            }
            else
            {                         /* chromatic case */
                                      /* set up magic numbers */
                if (luminosity <= (HLS_MAX / 2))
                {
                    magic2 = (int)((luminosity * ((int)HLS_MAX + saturation) + (HLS_MAX / 2)) / HLS_MAX);
                }
                else
                {
                    magic2 = luminosity + saturation - (int)(((luminosity * saturation) + (int)(HLS_MAX / 2)) / HLS_MAX);
                }
                magic1 = 2 * luminosity - magic2;

                /* get RGB, change units from HLSMax to RGBMax */
                r = (byte)(((HueToRGB(magic1, magic2, (int)(hue + (int)(HLS_MAX / 3))) * (int)RGB_MAX + (HLS_MAX / 2))) / (int)HLS_MAX);
                g = (byte)(((HueToRGB(magic1, magic2, hue) * (int)RGB_MAX + (HLS_MAX / 2))) / HLS_MAX);
                b = (byte)(((HueToRGB(magic1, magic2, (int)(hue - (int)(HLS_MAX / 3))) * (int)RGB_MAX + (HLS_MAX / 2))) / (int)HLS_MAX);
            }

#if __IOS__
            return UIColor.FromRGB(r / 255.0f, g / 255.0f, b / 255.0f);
#else
            return Android.Graphics.Color.Rgb(r, g, b);
#endif
        }

        int HueToRGB(int n1, int n2, int hue)
        {
            /* range check: note values passed add/subtract thirds of range */

            /* The following is redundant for WORD (unsigned int) */
            if (hue < 0)
            {
                hue += HLS_MAX;
            }

            if (hue > HLS_MAX)
            {
                hue -= HLS_MAX;
            }

            /* return r,g, or b value from this tridrant */
            if (hue < (HLS_MAX / 6))
            {
                return (n1 + (((n2 - n1) * hue + (HLS_MAX / 12)) / (HLS_MAX / 6)));
            }
            if (hue < (HLS_MAX / 2))
            {
                return (n2);
            }
            if (hue < ((HLS_MAX * 2) / 3))
            {
                return (n1 + (((n2 - n1) * (((HLS_MAX * 2) / 3) - hue) + (HLS_MAX / 12)) / (HLS_MAX / 6)));
            }
            else
            {
                return (n1);
            }

        }
        #endregion



    }
}