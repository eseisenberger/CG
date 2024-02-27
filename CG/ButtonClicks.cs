namespace CG;

public partial class MainWindow
{
    private void BlurClick(object sender, RoutedEventArgs e)
    {
        Queue.Add(Blur);
        ApplyNewest();
    }
    
    private void InverseClick(object sender, RoutedEventArgs e)
    {
        Queue.Add(Inversion);
        ApplyNewest();
    }

    private void BrightnessCorrectionClick(object sender, RoutedEventArgs e)
    {
        Queue.Add(BrightnessCorrection);
        ApplyNewest();
    }

    private void ContrastEnhancementClick(object sender, RoutedEventArgs e)
    {
        Queue.Add(ContrastEnhancement);
        ApplyNewest();
    }

    private void GammaCorrectionClick(object sender, RoutedEventArgs e)
    {
        Queue.Add(GammaCorrection);
        ApplyNewest();
    }
}