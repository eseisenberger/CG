using System;

namespace CG.Converters;

[ValueConversion(typeof(FunctionalFilter), typeof(string))]
public class EffectConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var effect = (FunctionalFilter?)value;
        return effect is null ? string.Empty : effect.Name;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
