using System.Text.Json.Serialization;

namespace backend.Models
{
    public class GeocodingApiResponse
    {
        [JsonPropertyName("results")]
        public List<GeocodingResult> Results { get; set; } = [];

        [JsonPropertyName("generationtime_ms")]
        public double GenerationTimeMs { get; set; }
    }
}