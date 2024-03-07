namespace CG.Extensions;

public static class PixelsEntensions
{
    public static void ToHsv(this byte[] pixels)
    {
        for (int i = 0; i < pixels.Length; i += 4)
        {
            var (r, g, b) = pixels.GetValues(i);
            var (h, s, v) = RgbToHsv(r, g, b);
            pixels.SetValues(i, h, s, v);
        }
    }
    
    public static void ToRgb(this byte[] pixels)
    {
        for (int i = 0; i < pixels.Length; i += 4)
        {
            var (r, g, b) = pixels.GetValues(i);
            var (h, s, v) = HsvToRgb(r, g, b);
            pixels.SetValues(i, h, s, v);
        }
    }

    private static double Max(double a, double b, double c) => a > b ? a > c ? a : c : b > c ? b : c;
    private static double Min(double a, double b, double c) => a < b ? a < c ? a : c : b < c ? b : c;
    private static (byte, byte, byte) RgbToHsv(byte originalR, byte originalG, byte originalB)
    {
        const double TOLERANCE = 0.001;
        double h = 0, s = 0, v = 0;
        double r = originalR / 255.0;
        double g = originalG / 255.0;
        double b = originalB / 255.0;
        double max = Max(r, g, b);
        double min = Min(r, g, b);
        double delta = max - min;
        if (Math.Abs(max - min) < TOLERANCE)
            h = 0;
        else if (Math.Abs(max - r) < TOLERANCE)
            h = 60 * (g - b) / delta + 360 % 360;
        else if (Math.Abs(max - g) < TOLERANCE)
            h = 60 * (b - r) / delta + 120 % 360;
        else if (Math.Abs(max - b) < TOLERANCE)
            h = 60 * (r - g) / delta + 240 % 360;
        if (max < TOLERANCE)
            s = 0;
        else
            s = (delta / max) * 100;
        v = max * 100;
        h *= 255 / 360.0;
        return ((byte, byte, byte))(h, s, v);
    }
    
    private static (byte r, byte g, byte b) HsvToRgb(byte h, byte s, byte v)
    {
        byte r, g, b;
        byte region, remainder, p, q, t;
    
        if (s == 0)
        {
            r = v;
            g = v;
            b = v;
            return (r, g, b);
        }
    
        region = (byte)(h / 43);
        remainder = (byte)((h - (region * 43)) * 6); 
    
        p = (byte)((v * (255 - s)) >> 8);
        q = (byte)((v * (255 - ((s * remainder) >> 8))) >> 8);
        t = (byte)((v * (255 - ((s * (255 - remainder)) >> 8))) >> 8);
    
        switch (region)
        {
            case 0:
                r = v; g = t; b = p;
                break;
            case 1:
                r = q; g = v; b = p;
                break;
            case 2:
                r = p; g = v; b = t;
                break;
            case 3:
                r = p; g = q; b = v;
                break;
            case 4:
                r = t; g = p; b = v;
                break;
            default:
                r = v; g = p; b = q;
                break;
        }
        return (r, g, b); 
    }

    public static (byte, byte, byte) GetValues(this byte[] pixels, int index)
    {
        return (pixels[index], pixels[index + 1], pixels[index + 2]);
    }
    
    public static void SetValues(this byte[] pixels, int index, byte r, byte g, byte b)
    {
        pixels[index] = r;
        pixels[index + 1] = g;
        pixels[index + 2] = b;
    }
}