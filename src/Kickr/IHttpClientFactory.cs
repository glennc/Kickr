﻿using System;
using System.Net.Http;

namespace Kickr
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient();
    }
}
