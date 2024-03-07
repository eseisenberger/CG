using Newtonsoft.Json;

namespace CG.Extensions;

public static class FilterExtensions
{
    public static string ToJson(this ConvolutionalFilterData data)
    {
        return JsonConvert.SerializeObject(data, Formatting.Indented);
    }

    public static ConvolutionalFilterData? GetFilterData(string text)
    {
        var result = JsonConvert.DeserializeObject<ConvolutionalFilterData>(text);
        return result;
    }
}