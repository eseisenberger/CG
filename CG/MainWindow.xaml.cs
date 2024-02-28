global using System.ComponentModel;
global using System.IO;
global using System.Runtime.CompilerServices;
global using System.Windows;
global using System.Windows.Media.Imaging;
global using Microsoft.Win32;
global using System.Globalization;
global using System.Windows.Data;
global using System.Collections.ObjectModel;
global using System.Windows.Controls;
global using CG.Classes;
using System.Windows.Input;

namespace CG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string FileTypeFilter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
        private const string DefaultImageSource = @"..\..\..\Resources\default.png";

        private const int Brightness = 20;
        private const int Contrast = 20;
        private const double Gamma = 2.2;
        private const int Kernel = 3;
        private const int ScrollRate = 10; //Scaling from 1 to 2 takes ScrollRate turns of the scroll wheel
        private static readonly int[,] SharpeningKernel =
        {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 }
        };

        private byte[] GammaCorrectionTable { get; } = new byte[256];
        public ObservableCollection<Effect> Queue { get; set; } = [];

        private WriteableBitmap _original = null!;
        private WriteableBitmap _modified = null!;
        private Effect _selectedEffect;
        private double _scale = 1.0;

        public WriteableBitmap Original
        {
            get => _original;
            set => SetField(ref _original, value);
        }
        public WriteableBitmap Modified
        {
            get => _modified;
            set => SetField(ref _modified, value);
        }

        public Effect SelectedEffect
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

        private void ApplyNewest()
        {
            var effect = Queue.FirstOrDefault();
            effect?.Apply(Modified);
        }
        
        private void Refresh()
        {
            var image = new WriteableBitmap(Original);
            foreach (var effect in Queue)
            {
                effect.Apply(image);
                Modified = image;
            }
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