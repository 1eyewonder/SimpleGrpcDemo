using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using GrpcService.Server.Helpers;
using GrpcService.Server.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GrpcService.Server.ProtoServices
{
    public class LocationService : LocationSearch.LocationSearchBase
    {
        private readonly ILogger<LocationService> _logger;
        private readonly IJsonParsingService _jsonParsingService;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public LocationService(ILogger<LocationService> logger,
            IHttpClientFactory httpClientFactory,
            IJsonParsingService jsonParsingService,
            IMapper mapper)
        {
            _logger = logger;
            _jsonParsingService = jsonParsingService;
            _mapper = mapper;
            _httpClient = httpClientFactory.CreateClient(StaticStrings.WeatherApi);
        }

        public override async Task GetLocationByStringContains(
            GetLocationStringRequest request, 
            IServerStreamWriter<LocationResponse> responseStream,
            ServerCallContext context)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_httpClient.BaseAddress}api/location/search/?query={request.Text}");

                response.EnsureSuccessStatusCode();

                var jArray = JsonConvert.DeserializeObject<JArray>(
                    await response.Content.ReadAsStringAsync());

                var locations = _jsonParsingService.GetLocations(jArray); 
                foreach (var location in locations)
                {
                    // Simulating long running task
                    await Task.Delay(1000);
                    await responseStream.WriteAsync(_mapper.Map<LocationResponse>(location));
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e,
                    "{Method} encountered an API exception: {Message}",
                    "GetLocationByStringContains",
                    e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "{Method} encountered an uncaught exception: {Message}",
                    "GetLocationByStringContains",
                    e.Message);
                throw;
            }
        }
    }
}