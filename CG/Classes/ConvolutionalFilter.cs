﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CG.Classes;

public class ConvolutionalFilter(ConvolutionalFilterData data) : IFilter, INotifyPropertyChanged
{
    private FilterState _state = FilterState.Pending;
    public string Name { get; set; } = data.Name;

    public FilterState State
    {
        get => _state;
        set => SetField(ref _state, value);
    }

    private ConvolutionalFilterData Data { get; set; } = data;
    private Func<BitmapData, ConvolutionalFilterData, Task<byte[]>> Func { get; } = ConvolutionalFilters.GetPixels;
    public async Task Apply(WriteableBitmap source)
    {
        var img = source.GetData();
        var pixels = await Func(img, Data);
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