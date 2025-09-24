using Microsoft.AspNetCore.Mvc;
using backend.Models;


namespace backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherController(IConfiguration config, IHttpClientFactory httpClientFactory) : ControllerBase
  {
    private readonly string _geocodingBaseUrl = config["GeocodingApiBaseUrl"]
        ?? throw new Exception("Geocoding API base URL missing");

    private readonly string _weatherBaseUrl = config["WeatherApiBaseUrl"]
        ?? throw new Exception("Weather API base URL missing");
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

        [HttpGet("search/{city}")]
    public async Task<IActionResult> SearchCity(string city)
    {
      if (string.IsNullOrWhiteSpace(city))
        return BadRequest("City name is required.");

      try
      {
        string? _geocodingapiKey = config["GeocodingApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(_geocodingapiKey) || _geocodingapiKey == "YOUR_DEV_KEY_HERE")
        {
          _geocodingapiKey = Environment.GetEnvironmentVariable("GEOCODING_API_KEY");
        }

        if (string.IsNullOrWhiteSpace(_geocodingapiKey))
        {
          throw new Exception("Geocoding API key is not set in environment variables.");
        }

        string url = $"{_geocodingBaseUrl}/search?q={city}&api_key={_geocodingapiKey}";

        var geoResponse = await _httpClient.GetFromJsonAsync<List<GeocodingResult>>(url);

        if (geoResponse == null || geoResponse.Count == 0)
          return NotFound(new { message = "No cities found" });

        var result = geoResponse.Select(g => new
        {
          g.DisplayName,
          g.Lat,
          g.Lon
        }).ToList();

        return Ok(result);
      }
      catch (HttpRequestException ex)
      {
        return StatusCode(500, new { message = "Error calling geocoding API", detail = ex.Message });
      }
    }

    [HttpGet("get/{lat}/{lon}")]
    public async Task<IActionResult> GetWeather(double lat, double lon)
    {
      try
      {
        string weatherUrl = $"{_weatherBaseUrl}/forecast?latitude={lat}&longitude={lon}"
                          + "&daily=weather_code,"
                          + "sunrise,sunset,"
                          + "temperature_2m_max,temperature_2m_min,"
                          + "apparent_temperature_max,apparent_temperature_min,"
                          + "uv_index_max,"
                          + "precipitation_probability_max,"
                          + "wind_speed_10m_max,wind_gusts_10m_max,wind_direction_10m_dominant"
                          + "&current=temperature_2m,"
                          + "precipitation,"
                          + "weather_code,"
                          + "apparent_temperature,"
                          + "wind_speed_10m,wind_direction_10m,wind_gusts_10m"
                          + "&timezone=auto&timeformat=unixtime";
                          
        var weatherResponse = await _httpClient.GetFromJsonAsync<object>(weatherUrl);

        return Ok(weatherResponse);
      }
      catch (HttpRequestException ex)
      {
        return StatusCode(500, new { message = "Error fetching weather", detail = ex.Message });
      }
    }
  }
}
