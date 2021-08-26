using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Server.Helpers;

namespace GrpcService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var locationClient = new LocationSearch.LocationSearchClient(channel);

            var locationRequested = new GetLocationStringRequest() { Text = "san" };

            using var call = locationClient.GetLocationByStringContains(locationRequested);
            while (await call.ResponseStream.MoveNext())
            {
                var currentLocation = call.ResponseStream.Current;
                Console.WriteLine(currentLocation.ToJson());
            }
        }
    }
}