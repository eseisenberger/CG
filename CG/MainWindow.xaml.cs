global using System.ComponentModel;
global using System.IO;
global using System.Runtime.CompilerServices;
global using System.Windows;
global using System.Windows.Media.Imaging;
global using Microsoft.Win32;

namespace CG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string FileTypeFilter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
        private const string DefaultImageSource = @"..\..\..\default.png";

        private const int Brightness = 20;
        private const int Contrast = 20;
        private const double Gamma = 2.2;
        private const int Kernel = 3;
        private static readonly int[,] SharpeningKernel =
        {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 }
        };
        
        private byte[] GammaCorrectionTable { get; } = new byte[256];
        public List<Action<WriteableBitmap>> Queue { get; set; } = [];

        private WriteableBitmap _original = null!;
        public WriteableBitmap Original
        {
            get => _original;
            set => SetField(ref _original, value);
        }
        
        private WriteableBitmap _modified = null!;
        public WriteableBitmap Modified
        {
            get => _modified;
            set => SetField(ref _modified, value);
        }

        private void InitializeGammaTable()
        {
            for (var i = 0; i < 256; i++)
                GammaCorrectionTable[i] = (byte)(255 * Math.Pow(i / 255.0, 1.0 / Gamma));
        }
        
        public MainWindow()
        {
            DataContext = this;
            var source = new Uri(DefaultImageSource, UriKind.Relative);
            var image = new BitmapImage(source);
            Original = new WriteableBitmap(image);
            Modified = new WriteableBitmap(image);
            InitializeGammaTable();
            InitializeComponent();
        }

        private void ApplyNewest()
        {
            var effect = Queue.LastOrDefault();
            effect?.Invoke(Modified);
        }
        
        private void Refresh()
        {
            var image = new WriteableBitmap(Original);
            foreach (var effect in Queue)
            {
                effect.Invoke(image);
                Modified = image;
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
}