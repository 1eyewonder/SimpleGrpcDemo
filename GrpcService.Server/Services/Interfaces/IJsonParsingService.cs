using System.Collections.Generic;
using GrpcService.Server.Dtos;
using Newtonsoft.Json.Linq;

namespace GrpcService.Server.Services.Interfaces
{
    public interface IJsonParsingService
    {
        List<Location> GetLocations(JArray array);
        ConsolidatedWeather GetWeather(JObject obj);
    }
}