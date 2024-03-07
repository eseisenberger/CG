using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace CG.Windows;

public partial class ConvolutionalFilterWindow : Window, INotifyPropertyChanged
{
    private ConvolutionalFilterData _data;
    private const int MaxSize = 9;

    #region SHADY STACKOVERFLOW CODE
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    
    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
    }
    #endregion
    
    
    public ConvolutionalFilterWindow(ConvolutionalFilterData data)
    {
        Loaded += OnLoaded;
        Data = data;
        KernelItems = GetKernelItems(data.Kernel);
        InitializeComponent();
    }


    private ObservableCollection<KernelItem> GetKernelItems(int[,] kernel)
    {
        var width = Data.Kernel.Width();
        var height = Data.Kernel.Height();

        var startX = (int)((MaxSize - width) / 2.0);
        var endX = MaxSize - startX;
        var startY = (int)((MaxSize - height) / 2.0);
        var endY = MaxSize - startY;
        var kernelItems = new ObservableCollection<KernelItem>();
        for (var x = 0; x < MaxSize; x++)
        {
            for (var y = 0; y < MaxSize; y++)
            {
                if (x >= startX && x < endX && y >= startY && y < endY)
                    kernelItems.Add(new KernelItem(Data.Kernel[y - startY, x - startX], x, y));
                else
                    kernelItems.Add(new KernelItem(null, x, y));
            }
        }
        return kernelItems;
    }

    public ConvolutionalFilterData Data
    {
        get => _data;
        set => SetField(ref _data, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<KernelItem> KernelItems { get; set; }

    public class KernelItem(int? value, int column, int row)
    {
        public int? Value { get; set; } = value;
        public int Column { get; set; } = column;
        public int Row { get; set; } = row;
        public (int x, int y) Position { get; set; } = (column, row);
    }

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

   

    private void CancelClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void SaveClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ApplyClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    

    
}