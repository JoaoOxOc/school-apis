﻿using Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Base
{
    public class JWTSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }

    }
}
