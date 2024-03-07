global using Microsoft.Win32;
global using System.ComponentModel;
global using System.IO;
global using System.Runtime.CompilerServices;
global using System.Windows;
global using System.Windows.Media.Imaging;
global using System.Globalization;
global using System.Windows.Data;
global using System.Collections.ObjectModel;
global using System.Windows.Controls;
global using System.Windows.Input;
global using CG.Classes;
global using CG.Interfaces;
global using CG.Filters;
global using CG.Extensions;

namespace CG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private const string FileTypeFilter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
        private const string DefaultImageSource = @"..\..\..\Resources\default.png";

        private const int Brightness = 20;
        private const int Contrast = 20;
        private const double Gamma = 2.2;
        private const int ScrollRate = 10; //Scaling from 1 to 2 takes ScrollRate turns of the scroll wheel

        private byte[] GammaCorrectionTable { get; } = new byte[256];
        public ObservableCollection<IFilter> Queue { get; set; } = [];

        private WriteableBitmap _original;
        private WriteableBitmap _modified;
        private IFilter? _selectedEffect;
        private double _scale = 1.0;
        private bool _isApplying;


        public bool IsApplying
        {
            get => _isApplying;
            set => SetField(ref _isApplying, value);
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

        public MainWindow()
        {
            PreviewMouseWheel += OnMouseWheel;
            InitializeSampleImages();
            InitializeGammaTable();
            InitializeComponent();
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

        private async Task ApplyNewest()
        {
            var filter = Queue.FirstOrDefault();
            if (filter is null)
                return;

            filter.State = "In progress...";
            IsApplying = true;
            await filter.Apply(Modified);
            IsApplying = false;
            filter.State = "Applied";
        }
        
        private void Refresh()
        {
            var image = new WriteableBitmap(Original);
            foreach (var effect in Queue)
            {
                effect.Apply(image);
                Modified = image;
            }
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
    }
}