using Newtonsoft.Json;

namespace GrpcService.Server.Dtos
{
    public record WeatherSource
    {
        [JsonProperty("title")]
        public string Name { get; init; }
        
        [JsonProperty("url")]
        public string Url { get; init; }
    }
}