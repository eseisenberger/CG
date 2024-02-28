namespace CG;

public partial class MainWindow
{
    private void ClickBase(string name, Action<WriteableBitmap> action)
    {
        var effect = new Effect(name, action);
        Queue.Insert(0, effect);
        ApplyNewest();
    }
    
    private void BlurClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Blur", Blur);
    }
    
    private void SharpenClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Sharpen", Sharpen);
    }
    
    private void InverseClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Invert", Inversion);
    }

    private void BrightnessCorrectionClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Correct Brightness", BrightnessCorrection);
    }

    private void ContrastEnhancementClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Enhance Contrast", ContrastEnhancement);
    }

    private void GammaCorrectionClick(object sender, RoutedEventArgs e)
    {
        ClickBase("Correct Gamma", GammaCorrection);
    }
}