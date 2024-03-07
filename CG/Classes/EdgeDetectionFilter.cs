using CG.Interfaces;

namespace CG.Classes;

public class EdgeDetectionFilter(string name, Action<WriteableBitmap, int[,], int[,], int> action, int[,] kernelX, int[,] kernelY, int divisor = 1) : IFilter
{
    public string Name { get; set; } = name;
    private Action<WriteableBitmap, int[,], int[,], int> Action { get; set; } = action;
    private int[,] KernelX { get; set; } = kernelX;
    private int[,] KernelY { get; set; } = kernelY;
    private int Divisor { get; set; } = divisor;
    public void Apply(WriteableBitmap source)
    {
        Action.Invoke(source, KernelX, KernelY, divisor);
    }
}