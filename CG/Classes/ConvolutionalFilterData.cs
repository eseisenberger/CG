namespace CG.Classes;

public class ConvolutionalFilterData : INotifyPropertyChanged
{
    private int _divisor;

    public ConvolutionalFilterData(string name, Func<int, int, (int[,], int)> kernelGenerator, int offset = 0, int offsetX = 0, int offsetY = 0, bool divisorEnabled = false, bool autoCalculateDivisor = false)
    {
        Name = name;
        Offset = offset;
        OffsetX = offsetX;
        OffsetY = offsetY;
        DivisorEnabled = divisorEnabled;
        AutoCalculateDivisor = autoCalculateDivisor;
        KernelGenerator = kernelGenerator;
        (Kernel, Divisor) = kernelGenerator.Invoke(3, 3);
    }
    public string Name { get; set; }
    public int[,] Kernel { get; set; }

    public int Divisor
    {
        get => _divisor;
        set => SetField(ref _divisor, value);
    }

    public int Offset { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public bool DivisorEnabled { get; set; }
    public bool AutoCalculateDivisor { get; set; }
    public Func<int, int, (int[,], int)> KernelGenerator { get; set; }
    
    
    public bool IsBlur => DivisorEnabled && AutoCalculateDivisor;
    
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