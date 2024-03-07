using CG.Interfaces;

namespace CG.Classes;

public class FunctionalFilter(string name, Func<BitmapData, byte[]> func) : IFilter, INotifyPropertyChanged
{
    private string _state = "Pending";
    public string Name { get; set; } = name;

    public string State
    {
        get => _state;
        set => SetField(ref _state, value);
    }

    private Func<BitmapData, byte[]> Func { get; } = func;

    public async Task Apply(WriteableBitmap source)
    {
        var img = source.GetData();
        var pixels = await Task.Run(() => Func(img));
        source.WritePixels(pixels);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}