using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleSignonPage.Util
{
    public class Resource
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ApiSecret { get; set; }
    }
    public class IdentityServerConfig
    {
        public List<Resource> Resources { get; set; }
    }
}
