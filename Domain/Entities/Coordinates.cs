using System.Globalization;

namespace Domain.Entities;

/// <summary>
/// Координаты
/// </summary>
public class Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinates(string latitude, string longitude)
    {
        var lat = string.IsNullOrEmpty(latitude) ? "0" : latitude;
        var lon = string.IsNullOrEmpty(longitude) ? "0" : latitude;

        Latitude = double.Parse(lat, CultureInfo.InvariantCulture);
        Longitude = double.Parse(lon, CultureInfo.InvariantCulture);
    }

    public Coordinates()
    {
        
    }
}