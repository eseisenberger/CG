using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CG.Classes;

public class ConvolutionalFilterData : INotifyPropertyChanged
{
    private int _divisor;

    public ConvolutionalFilterData(ConvolutionalFilterData other)
    {
        Name = other.Name;
        Offset = other.Offset;
        OffsetX = other.OffsetX;
        OffsetY = other.OffsetY;
        KernelGenerator = other.KernelGenerator;
        Kernel = other.Kernel.GetCopy();
        Divisor = other.Divisor;
    }
    
    
    public ConvolutionalFilterData(string name, Func<int, int, (int[,], int)> kernelGenerator, int offset = 0, int offsetX = 0, int offsetY = 0)
    {
        Name = name;
        Offset = offset;
        OffsetX = offsetX;
        OffsetY = offsetY;
        KernelGenerator = kernelGenerator;
        (Kernel, Divisor) = kernelGenerator.Invoke(3, 3);
    }
    
    [JsonConstructor]
    public ConvolutionalFilterData(string name, Dictionary<(int, int), int[,]> savedKernels, int[,] kernel, int divisor, int offset = 0, int offsetX = 0, int offsetY = 0)
    {
        Name = name;
        Offset = offset;
        OffsetX = offsetX;
        OffsetY = offsetY;
        Kernel = kernel;
        if(savedKernels is not null)
            SavedKernels = savedKernels;
        if(!SavedKernels.ContainsKey((kernel.Height(), kernel.Width())))
            SavedKernels.Add((kernel.Height(), kernel.Width()), Kernel);
        Divisor = divisor;
        KernelGenerator = DefaultGenerator;
    }

    private (int[,], int) DefaultGenerator(int height, int width)
    {
        var key = (height, width);
        var divisor = Divisor;
        var kernel = SavedKernels.ContainsKey(key) ? SavedKernels[key] : GetIdentityKernel(height, width);
        return (kernel, divisor);
    }

    public string Name { get; set; }
    public int[,] Kernel { get; set; }
    public Dictionary<(int, int), int[,]> SavedKernels { get; set; } = [];

    public int Divisor
    {
        get => _divisor;
        set => SetField(ref _divisor, value);
    }

    public int Offset { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public bool[] IgnoreChannels { get; set; } = new bool[3];
    [JsonIgnore]
    public Func<int, int, (int[,], int)> KernelGenerator { get; set; }


    private static int[,] GetIdentityKernel(int height, int width)
    {
        var kernel = new int[height, width];
        kernel.Fill(0);
        kernel.SetCenter(1);
        return kernel;
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