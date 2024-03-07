using System;
using System.Collections.Generic;

namespace CG.Controls;

public partial class KernelGrid : UserControl, INotifyPropertyChanged
{
    private int[,] _kernel = new int[9,9];

    public KernelGrid()
    {
        var length = Kernel.GetLength(0);
        var height = Kernel.GetLength(1);
        for(var x = 0; x < length; x++)
            for(var y = 0; y < height; y++)
                KernelItems.Add(new KernelItem(Kernel[x,y], x, y));
        
        InitializeComponent();
    }

    public int[,] Kernel
    {
        get => _kernel;
        set => SetField(ref _kernel, value);
    }

    public ObservableCollection<KernelItem> KernelItems { get; set; } = [];

    private void ValueChanged(object sender, TextChangedEventArgs e)
    {
        Console.WriteLine(e.ToString());
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

public class KernelItem(int value, int column, int row)
{
    public int Value { get; set; } = value;
    public int Column { get; set; } = column;
    public int Row { get; set; } = row;
}