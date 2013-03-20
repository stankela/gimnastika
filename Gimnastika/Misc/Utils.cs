using System;
using System.Drawing;

namespace Gimnastika
{
    public class Utils
    {
        public static PointF mmToPixel(Graphics g, PointF mm)
        {
            PointF result = new PointF();
            result.X = mm.X * g.DpiX / 25.4f;
            result.Y = mm.Y * g.DpiY / 25.4f;
            return result;
        }

    }
}
