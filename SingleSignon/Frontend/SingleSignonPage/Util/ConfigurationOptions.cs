using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleSignonPage.Util
{
    public class ConfigurationOptions : IConfigurationOptions
    {
        private readonly IdentityServerConfig _identityServerConfigs;

        public ConfigurationOptions(IOptions<IdentityServerConfig> identityServerConfigs)
        {
            this._identityServerConfigs = identityServerConfigs.Value;
        }

        public IdentityServerConfig getIdentityServerConfig()
        {
            return this._identityServerConfigs;
        }
    }
}
