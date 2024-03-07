namespace CG;

public partial class MainWindow
{
    private static byte[] Inversion(BitmapData img)
    {
        return img.Pixels.Select(p => (byte)(255 - p)).ToArray();
    }

    private static byte[] BrightnessCorrection(BitmapData img)
    {
        return img.Pixels.Select(p => (byte)int.Clamp(p + Brightness, 0, 255)).ToArray();
    }

    private static byte[] ContrastEnhancement(BitmapData img)
    {
        var contrast = Math.Pow((100 + Contrast) / 100.0, 2);
        return img.Pixels.Select(p => 
            (byte)int.Clamp((int)((p / 255.0 - 0.5) * contrast + 0.5) * 255, 0, 255)
        ).ToArray();
    }

    private byte[] GammaCorrection(BitmapData img)
    {
        return img.Pixels.Select(p => GammaCorrectionTable[p]).ToArray();
    }
}