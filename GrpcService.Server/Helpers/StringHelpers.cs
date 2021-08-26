using Newtonsoft.Json;

namespace GrpcService.Server.Helpers
{
    public static class StringHelpers
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });
        }
    }
}