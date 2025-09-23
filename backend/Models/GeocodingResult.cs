namespace backend.Models
{
    public class GeocodingResult
    {
        public string Name { get; set; } = string.Empty;
        public string? Admin1 { get; set; } // state / region
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}