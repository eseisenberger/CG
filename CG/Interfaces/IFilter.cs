namespace CG.Interfaces;

public interface IFilter
{
    string Name { get; set; }
    string State { get; set; }
    Task Apply(WriteableBitmap source);
}