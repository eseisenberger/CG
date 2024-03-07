using System;
using System.Diagnostics;
using CG.Enums;

namespace CG.Converters;

public class FilterStateToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var state = (FilterState?)value;
        switch(state)
        {
            case FilterState.Pending:
                return "Pending";
            case FilterState.InProgress:
                return "In progress...";
            case FilterState.Done:
                return "Done";
            default:
                return "State Conversion Error";
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}