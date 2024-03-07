using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    private const string FileTypeFilter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
    private const string DefaultImageSource = @"..\..\..\Resources\default.png";
    private const string CustomFiltersLocation = @"..\..\..\Resources\CustomFilters";

    private const int Brightness = 20;
    private const int Contrast = 20;
    private const double Gamma = 2.2;
    private const int ScrollRate = 10; //Scaling from 1 to 2 takes ScrollRate turns of the scroll wheel

    private byte[] GammaCorrectionTable { get; } = new byte[256];
    public ObservableCollection<IFilter> Queue { get; set; } = [];
    public ObservableCollection<ConvolutionalFilterData> CustomFilters { get; set; } = [];
    private WriteableBitmap _original;
    private WriteableBitmap _modified;
    private IFilter? _selectedEffect;
    private double _scale = 1.0;
    private bool _isApplying;
    private int _totalPixels;
    private int _currentPixel;


    public bool IsApplying
    {
        get => _isApplying;
        set => SetField(ref _isApplying, value);
    }

    public int CurrentPixel
    {
        get => _currentPixel;
        set => SetField(ref _currentPixel, value);
    }

    public int TotalPixels
    {
        get => _totalPixels;
        set => SetField(ref _totalPixels, value);
    }

    public WriteableBitmap Original
    {
        get => _original;
        private set => SetField(ref _original, value);
    }
    public WriteableBitmap Modified
    {
        get => _modified;
        private set => SetField(ref _modified, value);
    }

    public IFilter? SelectedEffect
    {
        get => _selectedEffect;
        set => SetField(ref _selectedEffect, value);
    }

    public double Scale
    {
        get => _scale;
        set => SetField(ref _scale, value);
    }

    public bool[] IgnoreChannels { get; set; } = new bool[3];

    public MainWindow()
    {
        PreviewMouseWheel += OnMouseWheel;
        InitializeSampleImages();
        InitializeGammaTable();
        InitializeComponent();
        Directory.CreateDirectory(CustomFiltersLocation);
        LoadCustomFilters();
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            Scale = Math.Clamp(Scale + e.Delta / (ScrollRate * 120.0), 0.5, 2.0);
        }
    }

    private void InitializeSampleImages()
    {
        var source = new Uri(DefaultImageSource, UriKind.Relative);
        var image = new BitmapImage(source);
        Original = new WriteableBitmap(image);
        Modified = new WriteableBitmap(image);
    }

    private void InitializeGammaTable()
    {
        for (var i = 0; i < 256; i++)
            GammaCorrectionTable[i] = (byte)(255 * Math.Pow(i / 255.0, 1.0 / Gamma));
    }

    private async Task ApplyPendingFilters()
    {
        if (IsApplying)
            return;
            
        IsApplying = true;
        var filter = Queue.LastOrDefault(f => f.State.Equals(FilterState.Pending));
        while (filter != default)
        {
            filter.State = FilterState.InProgress;
            await filter.Apply(Modified);
            filter.State = FilterState.Done;
            filter = Queue.LastOrDefault(f => f.State.Equals(FilterState.Pending));
        }
        IsApplying = false;
    }
        
    private async Task Refresh()
    {
        var image = new WriteableBitmap(Original);
        foreach (var filter in Queue)
            filter.State = FilterState.Pending;
        await ApplyPendingFilters();
        Modified = image;
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

    private void OnScroll(object sender, ScrollChangedEventArgs e)
    {
        if (OriginalScroll is null || ModifiedScroll is null)
        {
            Error("ScrollViewers are set up incorrectly in the presentation layer.");
            return;
        }
            
        OriginalScroll.ScrollToVerticalOffset(e.VerticalOffset);
        OriginalScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
            
        ModifiedScroll.ScrollToVerticalOffset(e.VerticalOffset);
        ModifiedScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
    }

    private void HsvClick(object sender, RoutedEventArgs e)
    {
        var source = Modified;
        var pixels = source.GetPixels();
        pixels.ToHsv();
        source.WritePixels(pixels);
    }

    private void RgbClick(object sender, RoutedEventArgs e)
    {
        var source = Modified;
        var pixels = source.GetPixels();
        pixels.ToRgb();
        source.WritePixels(pixels);
    }
}