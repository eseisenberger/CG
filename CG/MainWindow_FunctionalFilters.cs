namespace CG;

public partial class MainWindow
{
    private static void Inversion(WriteableBitmap source)
    {
        var pixels = source.GetPixels();
        var result = pixels.Select(p => (byte)(255 - p)).ToArray();
        source.WritePixels(result);
    }

    private static void BrightnessCorrection(WriteableBitmap source)
    {
        var pixels = source.GetPixels();
        var result = pixels.Select(p => (byte)int.Clamp(p + Brightness, 0, 255)).ToArray();
        source.WritePixels(result);
    }

    private static void ContrastEnhancement(WriteableBitmap source)
    {
        var contrast = Math.Pow((100 + Contrast) / 100.0, 2);
        var pixels = source.GetPixels();
        var result = pixels.Select(p => 
            (byte)int.Clamp((int)((p / 255.0 - 0.5) * contrast + 0.5) * 255, 0, 255)
        ).ToArray();
        source.WritePixels(result);
    }

    private void GammaCorrection(WriteableBitmap source)
    {
        var pixels = source.GetPixels();
        var result = pixels.Select(p => GammaCorrectionTable[p]).ToArray();
        source.WritePixels(result);
    }
}