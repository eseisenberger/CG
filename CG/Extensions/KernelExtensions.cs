using System;

namespace CG.Extensions;

public static class KernelExtensions
{

    public static (int height, int width) GetDimensions(this int[,] kernel)
    {
        var height = kernel.GetLength(0);
        var width = kernel.GetLength(1);
        return (height, width);
    }
    public static void Fill(this int[,] kernel, int value)
    {
        var (height, width) = kernel.GetDimensions();
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                kernel[y, x] = value;
    }
    
    public static void SetCenter(this int[,] kernel, int value)
    {
        var (height, width) = kernel.GetDimensions();
        kernel[height / 2, width / 2] = value;
    }
    
    public static void FillCenterHorizontally(this int[,] kernel, int value)
    {
        var (height, width) = kernel.GetDimensions();
        for(var x = 0; x < width; x++)
            kernel[height / 2, x] = value;
    }
    
    public static void FillCenterVertically(this int[,] kernel, int value)
    {
        var (height, width) = kernel.GetDimensions();
        for(var y = 0; y < height; y++)
            kernel[y, width / 2] = value;
    }

    public static int Sum(this int[,] kernel)
    {
        var (height, width) = kernel.GetDimensions();
        var sum = 0;
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                sum += kernel[y, x];
        return sum;
    }

    public static int Height(this int[,] kernel) => kernel.GetLength(0);
    public static int Width(this int[,] kernel) => kernel.GetLength(1);

    public static int[,] GetCopy(this int[,] kernel)
    {
        var copy = new int[kernel.Height(), kernel.Width()];
        Array.Copy(kernel, copy, kernel.Length);
        return copy;
    }
}