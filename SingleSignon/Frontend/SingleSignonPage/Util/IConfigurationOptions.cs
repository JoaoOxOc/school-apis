using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleSignonPage.Util
{
    public interface IConfigurationOptions
    {
        IdentityServerConfig getIdentityServerConfig();
    }
}
