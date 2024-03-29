﻿using System;
using System.Threading.Tasks;

namespace CG.Windows;

public partial class MainWindow
{
    private async Task ApplyFunctionalFilter(string name, Func<BitmapData, byte[]> func)
    {
        var effect = new FunctionalFilter(name, func);
        Queue.Insert(0, effect);
        await ApplyPendingFilters();
    }
    
    #region FUNCTIONAL
    private async void InverseClick(object sender, RoutedEventArgs e)
    {
        await ApplyFunctionalFilter("Invert", MainWindow.Inversion);
    }

    private async void BrightnessCorrectionClick(object sender, RoutedEventArgs e)
    {
        await ApplyFunctionalFilter("Correct Brightness", MainWindow.BrightnessCorrection);
    }

    private async void ContrastEnhancementClick(object sender, RoutedEventArgs e)
    {
        await ApplyFunctionalFilter("Enhance Contrast", MainWindow.ContrastEnhancement);
    }

    private async void GammaCorrectionClick(object sender, RoutedEventArgs e)
    {
        await ApplyFunctionalFilter("Correct Gamma", GammaCorrection);
    }
    #endregion
    
    #region CONVOLUTIONAL

    private async Task ApplyConvolutionalFilter(ConvolutionalFilterData data)
    {
        var window = new ConvolutionalFilterWindow(new ConvolutionalFilterData(data), CustomFiltersLocation, this); 
        var dialogResult = window.ShowDialog();
        if (dialogResult != true)
            return;
        
        var windowData = window.Data;
        var filter = new ConvolutionalFilter(windowData);
        Queue.Insert(0, filter);
        await ApplyPendingFilters();
    }

    private async void CustomFilterClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;
        if (button.Tag is not ConvolutionalFilterData data)
            return;
        await ApplyConvolutionalFilter(data);
    }
    
    private async void BlurClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Blur", 
            kernelGenerator: ConvolutionalFilters.MakeBlurKernel
        );
        await ApplyConvolutionalFilter(data); 
    }
    private async void SharpenClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Sharpening", 
            kernelGenerator: ConvolutionalFilters.MakeSharpeningKernel
        );
        await ApplyConvolutionalFilter(data); 
    }
    private async void GaussianBlurClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Gaussian Blur",
            kernelGenerator: ConvolutionalFilters.MakeGaussKernel
        );
        await ApplyConvolutionalFilter(data); 
    }
    
    private async void EmbossClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Embossing", 
            kernelGenerator: ConvolutionalFilters.MakeEmbossKernel,
            offset: 128
        );
        await ApplyConvolutionalFilter(data); 
    }
    private async void EdgeDetectionClick(object sender, RoutedEventArgs e)
    {
        var data = new ConvolutionalFilterData
        (
            name: "Edge Detection", 
            kernelGenerator: ConvolutionalFilters.MakeEdgeDetectionKernel
        );
        await ApplyConvolutionalFilter(data); 
    }
    #endregion


    #region UTILITY
    private async void RemoveClick(object sender, RoutedEventArgs e)
    {
        if (SelectedEffect is null || SelectedEffect.State.Equals("In progress..."))
            return;

        Queue.Remove(SelectedEffect);
        SelectedEffect = null;
        
        await Refresh();
    }
    #endregion
    
}