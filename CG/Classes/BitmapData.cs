namespace CG.Classes;

public class BitmapData(BitmapSource source)
{
    public readonly int Height = source.PixelHeight;
    public readonly int Width = source.PixelWidth;
    public readonly int Stride = source.Stride();
    public readonly byte[] Pixels = source.GetPixels();
    public byte[] OriginalPixels = source.GetPixels();
}