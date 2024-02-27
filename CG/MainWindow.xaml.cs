using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string FileTypeFilter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

        private BitmapImage? _original;
        public BitmapImage? Original
        {
            get => _original;
            set => SetField(ref _original, value);
        }
        
        private BitmapImage? _modified;
        public BitmapImage? Modified
        {
            get => _modified;
            set => SetField(ref _modified, value);
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = FileTypeFilter
            };
            
            if (dialog.ShowDialog() != true)
                return;
            
            var file = new Uri(dialog.FileName);
            Original = Modified = new(file);

        }

        private void Error(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (Original is not BitmapSource image)
                return;
            

            SaveFileDialog dialog = new()
            {
                Filter = "PNG Image|*.png",
                FileName = "Image",
                DefaultExt = ".png"
            };
            if (dialog.ShowDialog() == false)
                return;

            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using var stream = File.Create(dialog.FileName);
            encoder.Save(stream);
        }

        private void BlurClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InverseClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BrightnessCorrectionClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ContrastEnhancementClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GammaCorrectionClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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