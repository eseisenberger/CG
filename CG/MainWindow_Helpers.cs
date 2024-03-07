namespace CG;

public partial class MainWindow
{
    private void Error(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    
    
    private void OpenFile(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            Filter = FileTypeFilter
        };
            
        if (dialog.ShowDialog() != true)
            return;
        
        Queue.Clear();
        
        var file = new Uri(dialog.FileName);
        var image = new BitmapImage(file);
        Original = new WriteableBitmap(image);
        Modified = new WriteableBitmap(image);
    }

    private void SaveFile(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new()
        {
            Filter = "PNG Image|*.png",
            FileName = "Image",
            DefaultExt = ".png"
        };
        if (dialog.ShowDialog() == false)
            return;

        PngBitmapEncoder encoder = new();
        encoder.Frames.Add(BitmapFrame.Create(Modified));
        using var stream = File.Create(dialog.FileName);
        encoder.Save(stream);
    }

    private void ResetImage(object sender, RoutedEventArgs e)
    {
        Queue.Clear();
        Modified = new WriteableBitmap(Original);
    }

    public void LoadCustomFilters()
    {
        CustomFilters.Clear(); 
        var files = Directory.EnumerateFiles(CustomFiltersLocation).Where(f => Path.GetExtension(f).Equals(".json"));
        foreach (var file in files)
        {
            using var stream = File.OpenRead(file);
            using var sr = new StreamReader(stream);
            var json = sr.ReadToEnd();
            var data = FilterExtensions.GetFilterData(json);
            if (data is null)
                continue;
            CustomFilters.Add(data);
        }
    }
    
}