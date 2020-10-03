using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleSignonPage.Util
{
    public class IdentityServerResource
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ApiSecret { get; set; }
    }
    public class IdentityServerClient
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ClientSecret { get; set; }
    }

    public class IdentityServerConfig
    {
        public List<IdentityServerClient> Clients { get; set; }
        public List<IdentityServerResource> Resources { get; set; }
    }
}
