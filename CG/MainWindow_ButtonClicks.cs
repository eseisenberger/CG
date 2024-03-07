using CG.Filters;
using CG.Windows;

namespace CG;

public partial class MainWindow
{
    private void ApplyFunctionalFilter(string name, Action<WriteableBitmap> action)
    {
        var effect = new FunctionalFilter(name, action);
        Queue.Insert(0, effect);
        ApplyNewest();
    }
    
    #region FUNCTIONAL
    private void InverseClick(object sender, RoutedEventArgs e)
    {
        ApplyFunctionalFilter("Invert", Inversion);
    }

    private void BrightnessCorrectionClick(object sender, RoutedEventArgs e)
    {
        ApplyFunctionalFilter("Correct Brightness", BrightnessCorrection);
    }

    private void ContrastEnhancementClick(object sender, RoutedEventArgs e)
    {
        ApplyFunctionalFilter("Enhance Contrast", ContrastEnhancement);
    }

    private void GammaCorrectionClick(object sender, RoutedEventArgs e)
    {
        ApplyFunctionalFilter("Correct Gamma", GammaCorrection);
    }
    #endregion
    
    #region CONVOLUTIONAL

    private void ApplyConvolutionalFilter(ConvolutionalFilterData data)
    {
        var window = new ConvolutionalFilterWindow(data); 
        var dialogResult = window.ShowDialog();
        if (dialogResult != true)
            return;
        
        var windowData = window.Data;
        var filter = new ConvolutionalFilter(windowData, ConvolutionalFilters.Apply);
        Queue.Insert(0, filter);
        ApplyNewest();
    }
    
    private void BlurClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Blur", 
            kernelGenerator: ConvolutionalFilters.MakeBlurKernel,
            divisorEnabled: true, 
            autoCalculateDivisor: true
        );
        ApplyConvolutionalFilter(data); 
    }
    private void SharpenClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Sharpening", 
            kernelGenerator: ConvolutionalFilters.MakeSharpeningKernel
        );
        ApplyConvolutionalFilter(data); 
    }
    private void GaussianBlurClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Gaussian Blur",
            kernelGenerator: ConvolutionalFilters.MakeGaussKernel,
            divisorEnabled: true, 
            autoCalculateDivisor: false
        );
        ApplyConvolutionalFilter(data); 
    }
    
    private void EmbossClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Embossing", 
            kernelGenerator: ConvolutionalFilters.MakeEmbossKernel,
            offset: 128
        );
        ApplyConvolutionalFilter(data); 
    }
    private void EdgeDetectionClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Edge Detection", 
            kernelGenerator: ConvolutionalFilters.MakeEdgeDetectionKernel
        );
        ApplyConvolutionalFilter(data); 
    }
    #endregion


    #region UTILITY
    private void RemoveClick(object sender, RoutedEventArgs e)
    {
        if (SelectedEffect is null)
            return;

        Queue.Remove(SelectedEffect);
        SelectedEffect = null;
        
        Refresh();
    }
    #endregion
    
}