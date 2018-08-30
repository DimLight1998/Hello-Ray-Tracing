using System;
using System.Drawing;

namespace HelloRayTracing
{
    public static class Utils
    {
        public static Color Multiply(this Color color, float value)
        {
            var r = Math.Clamp((int) (color.R * value), 0, 255);
            var g = Math.Clamp((int) (color.G * value), 0, 255);
            var b = Math.Clamp((int) (color.B * value), 0, 255);
            var a = Math.Clamp((int) (color.A * value), 0, 255);
            return Color.FromArgb(a, r, g, b);
        }

        public static Color Moderate(this Color color, Color other)
        {
            var r = color.R * other.R / 255;
            var g = color.G * other.G / 255;
            var b = color.B * other.B / 255;
            var a = color.A * other.A / 255;
            return Color.FromArgb(a, r, g, b);
        }

        public static Color Add(this Color color, Color other)
        {
            var r = Math.Clamp(color.R + other.R, 0, 255);
            var g = Math.Clamp(color.G + other.G, 0, 255);
            var b = Math.Clamp(color.B + other.B, 0, 255);
            var a = Math.Clamp(color.A + other.A, 0, 255);
            return Color.FromArgb(a, r, g, b);
        }
    }
}