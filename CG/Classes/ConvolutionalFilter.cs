using CG.Interfaces;

namespace CG.Classes;

public class ConvolutionalFilter(ConvolutionalFilterData data, Action<WriteableBitmap, ConvolutionalFilterData> action) : IFilter
{
    public string Name { get; set; } = data.Name;
    private ConvolutionalFilterData Data { get; set; } = data;
    private Action<WriteableBitmap, ConvolutionalFilterData> Action { get; set; } = action;
    public void Apply(WriteableBitmap source)
    {
        Action.Invoke(source, Data);
    }
}