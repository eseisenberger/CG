namespace CG.Interfaces;

public interface IFilter
{
    public string Name { get; set; }
    public void Apply(WriteableBitmap source);
}