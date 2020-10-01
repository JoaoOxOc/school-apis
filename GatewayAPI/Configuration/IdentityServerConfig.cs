using System.Collections.Generic;

namespace GatewayAPI.Configuration
{
    public class IdentityServerConfig
    {
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string IdentityScheme { get; set; }
        public List<APIResource> Resources { get; set; }
    }

    public class APIResource
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public string ApiSecret { get; set; }
    }
}
