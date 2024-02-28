namespace CG.Classes;

public class Effect(string name, Action<WriteableBitmap> action)
{
    public string Name { get; set; } = name;
    private Action<WriteableBitmap> Action { get; set; } = action;

    public void Apply(WriteableBitmap source)
    {
        Action.Invoke(source);
    }
}