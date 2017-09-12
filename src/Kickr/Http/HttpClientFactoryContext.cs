using System;
using System.Net.Http;

namespace Kickr.Http
{
    public abstract class HttpClientFactoryContext
    {
        public abstract Uri BaseAddress { get; set; }

        public abstract  string ClientName { get; }

        public abstract HttpMessageHandler MessageHandler { get; set; }

        public abstract void PrependMessageHandler(DelegatingHandler messageHandler);
    }
}