using CG.Extensions;

namespace CG.Filters;

public static class ConvolutionalFilters
{
    public static void Apply(WriteableBitmap source, ConvolutionalFilterData data)
    {
        var img = source.GetData();
        
        for (var y = 0; y < img.Height; y++)
            for (var x = 0; x < img.Width; x++)
                Compute(y, x, ref img, data);
        
        source.WritePixels(img.Pixels);
    }
    
    private static void Compute(int x, int y, ref BitmapData img, ConvolutionalFilterData data)
    {
        var index = y * img.Stride + x * 4;
        var boundY = data.Kernel.Height() / 2;
        var boundX = data.Kernel.Width() / 2;

        for (var channel = 0; channel < 3; channel++)
        {
            var sum = 0;
            for (var ny = -boundY - data.OffsetY; ny <= boundY - data.OffsetY; ny++)
            {
                for (var nx = -boundX - data.OffsetX; nx <= boundX - data.OffsetX; nx++)
                {
                    var clampedY = Math.Clamp(y + ny, 0, img.Height - 1);
                    var clampedX = Math.Clamp(x + nx, 0, img.Width - 1);
                    var sample = clampedY * img.Stride  + clampedX * 4;
                    sum += img.OriginalPixels[sample + channel] * data.Kernel[ny + boundY + data.OffsetY, nx + boundX + data.OffsetX];
                }
            }

            img.Pixels[index + channel] = (byte)Math.Clamp((sum + data.Offset) / (double)data.Divisor, 0, 255);
        }
    }

    public static (int[,] Kernel, int Divisor) MakeBlurKernel(int height, int width)
    {
        var kernel = new int[height, width];

        kernel.Fill(1);
        
        return (kernel, height * width);
    }
    
    public static (int[,] Kernel, int Divisor) MakeSharpeningKernel(int height, int width)
    {
        var kernel = new int[height, width];

        kernel.Fill(-1);
        kernel.SetCenter(2 * width * height - 1);
        
        return (kernel, height * width);
    }
    
    public static (int[,] Kernel, int Divisor) MakeEdgeDetectionKernel(int height, int width)
    {
        var kernel = new int[height, width];
        
        kernel.Fill(-1);
        kernel.SetCenter(width * height - 1);
        
        return (kernel, 1);
    }

    public static (int[,] Kernel, int Divisor) MakeEmbossKernel(int height, int width)
    {
        var kernel = new int[height, width];
        
        kernel.Fill(0);
        for (var x = 0; x < width / 2; x++)
            kernel[height / 2, x] = 1;
        for (var x = width / 2 + 1; x < width ; x++)
            kernel[height / 2, x] = -1;
        kernel.SetCenter(0);

        return (kernel, 1);
    }
    
    public static (int[,] Kernel, int Divisor) MakeGaussKernel(int height, int width)
    {
        if (height != width)
            throw new ArgumentException();

        var sigma = Random.Shared.NextDouble() / 2;
        var radius = (width - 1) / 2;
        
        var temp = new double[width, width];
        var sum = 0.0;

        for (var x = -radius; x <= radius; x++)
        {
            for (var y = -radius; y <= radius; y++)
            {
                var distance = Math.Pow(x, 2) + Math.Pow(y, 2);
                var sigma2 = Math.Pow(sigma, 2);
                var result = Math.Exp(-distance / sigma2);
                
                var value = result / (2 * Math.PI * sigma2) * 1000;

                temp[x + radius, y + radius] = value;
                sum += value;
            }
        }

        var kernel = new int[width, width];
        for (var x = 0; x <= width - 1; x++)
            for (var y = 0; y <= width - 1; y++)
                kernel[y, x] = (int)(temp[y, x] * Math.Round(1000 / sum));

        return (kernel, kernel.Sum());
    }
}