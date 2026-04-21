using System.Globalization;

namespace task.Infrastructure.Utilities;

public static class InvariantDoubleParser
{
    public static double ParseOrZero(string? value) =>
        double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
            ? result
            : 0;
}
