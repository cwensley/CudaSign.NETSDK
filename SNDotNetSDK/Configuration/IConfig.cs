﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNDotNetSDK.Configuration
{
    public interface IConfig
    {
        string GetApiBase();

        string GetBase64EncodedClientCredentials();
    }
}
