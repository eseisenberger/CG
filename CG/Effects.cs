namespace CG;

public partial class MainWindow
{
    private void Inversion(WriteableBitmap source)
    {
        var pixels = source.GetPixels();
        var result = pixels.Select(p => (byte)(255 - p)).ToArray();
        source.WritePixels(result);
    }

    private void BrightnessCorrection(WriteableBitmap source)
    {
        var pixels = source.GetPixels();
        var result = pixels.Select(p => (byte)int.Clamp(p + Brightness, 0, 255)).ToArray();
        source.WritePixels(result);
    }
    
    private void ContrastEnhancement(WriteableBitmap source)
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
        var contrast = Math.Pow((100 + Contrast) / 100.0, 2);
        var pixels = source.GetPixels();
        var result = pixels.Select(p => GammaCorrectionTable[p]).ToArray();
        source.WritePixels(result);
    }
    
    
    //CONVOLUTIONAL

    private void Blur(WriteableBitmap source)
    {
        var kernel = Kernel;
        var bound = kernel / 2;
        var height = source.PixelHeight;
        var width = source.PixelWidth;
        var stride = source.Stride();
        var pixels = source.GetPixels();
        
        for (var y = bound; y < height - bound; y++)
            for (var x = bound; x < width - bound; x++)
                ApplyBlur(ref pixels, x, y, stride, kernel);
        
        source.WritePixels(pixels);
    }

    private static void ApplyBlur(ref byte[] pixels, int x, int y, int stride, int kernel)
    {
        var index = y * stride + x * 4;
        var bound = kernel / 2;
        
        for (var channel = 0; channel < 3; channel++)
        {
            var sum = 0;
            for (var ny = -bound; ny <= bound; ny++)
            {
                for (var nx = -bound; nx <= bound; nx++)
                {
                    var neighborIndex = (y + ny) * stride + (x + nx) * 4;
                    sum += pixels[neighborIndex + channel];
                }
            }
            pixels[index + channel] = (byte)(sum / 9.0);
        }
    }
}