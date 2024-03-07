using CG.Interfaces;

namespace CG.Classes;

public class FunctionalFilter(string name, Action<WriteableBitmap> action) : IFilter
{
    public string Name { get; set; } = name;
    private Action<WriteableBitmap> Action { get; } = action;

    public void Apply(WriteableBitmap source)
    {
        Action.Invoke(source);
    }
}