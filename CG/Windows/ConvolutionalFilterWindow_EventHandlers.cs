namespace CG.Windows;

public partial class ConvolutionalFilterWindow
{
    private int? ToInt(string text)
    {
        var negative = false;
        if (text.First() == '-')
        {
            negative = true;
            text = text[1..];
        }
        if (!text.All(char.IsNumber) || text.Length == 0)
            return null;

        return Convert.ToInt32(text) * (negative ? -1 : 1);
    }
    
    private void KernelValueChanged(object sender, TextChangedEventArgs textChangedEventArgs)
    {
        var textBox = sender as TextBox;
        var text = textBox?.Text;
        if (text is null)
            return;

        var value = ToInt(text);
        if (value is null)
            return;
        
        var presenter = textBox?.TemplatedParent as ContentPresenter;
        var tag = presenter?.Tag;
        if (tag is not (int, int ))
            return;
        var pos = ((int, int)) tag;
        var (x, y) = pos;
        var offset = (int)((MaxSize - Data.Kernel.GetLength(0)) / 2.0);
        x -= offset;
        y -= offset;
        
        Data.Kernel[x, y] = (int)value;
    }
    
    private void OffsetYChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var text = textBox?.Text;
        if (text is null || !text.All(char.IsNumber)) 
            return;
        
        var value = Convert.ToInt32(text);
        Data.OffsetY = value;
    }
    
    private void OffsetXChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var text = textBox?.Text;
        if (text is null || !text.All(char.IsNumber)) 
            return;
        
        var value = Convert.ToInt32(text);
        Data.OffsetX = value;
    }
    
    private void DivisorChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var text = textBox?.Text;
        if (text is null || !text.All(char.IsNumber)) 
            return;
        
        var value = Convert.ToInt32(text);
        Data.Divisor = value;
    }

    private void OffsetChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var text = textBox?.Text;
        if (text is null || !text.All(char.IsNumber)) 
            return;
        
        var value = Convert.ToInt32(text);
        Data.Offset = value;
    }
    
    private void KernelRowCountChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        var item = comboBox?.SelectedItem as ComboBoxItem;
        if (item?.Content is not string text)
            return;
        
        var value = ToInt(text);
        if (value is null)
            return;

        var height = (int)value;
        var width = Data.Kernel.Width();
        if (height == Data.Kernel.Height())
            return;

        SetKernelSize(height, width);
    }
    
    private void KernelColumnCountChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        var item = comboBox?.SelectedItem as ComboBoxItem;
        if (item?.Content is not string text)
            return;
        
        var value = ToInt(text);
        if (value is null)
            return;

        var height = Data.Kernel.Height();
        var width = (int)value;
        if (width == Data.Kernel.Width())
            return;

        SetKernelSize(height, width);
    }
    
    private void SetKernelSize(int height, int width)
    {
        var (kernel, divisor) = Data.KernelGenerator.Invoke(height, width);

        Data.Kernel = kernel;
        Data.Divisor = divisor;
        
        KernelItems.Clear();
        foreach(var el in GetKernelItems(Data.Kernel))
            KernelItems.Add(el);
    }
}