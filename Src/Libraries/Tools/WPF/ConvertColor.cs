using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Tools.WPF
{
    public static class ConvertColor
    {

        public static ScRgb ToScRgB(this Color color)
        {
            return ToScRgbImpl(color);
        }

        public static ScRgb ToScRgbImpl(Color color)
        {
            return new ScRgb(color.ScA, color.ScR, color.ScG, color.ScB);
        }
    }
    
    [Serializable]
    public class ScRgb
    {
        public float ScA { get;  set; }
        public float ScR { get;  set; }
        public float ScG { get;  set; }
        public float ScB { get;  set; }

        public ScRgb()
            : this(0xFF, 0, 0, 0)
        {
        }

        public ScRgb(float scA, float scR, float scG, float scB)
        {
            ScA = scA;
            ScR = scR;
            ScB = scB;
            ScG = scG;
        }

        public Color ToColor()
        {
            return Color.FromScRgb(ScA, ScR, ScG, ScB);
        }
    }
}
