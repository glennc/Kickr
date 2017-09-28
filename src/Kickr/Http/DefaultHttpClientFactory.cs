using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Kickr.Http
{
    internal class DefaultHttpClientFactory : HttpClientFactory
    {
        private readonly IHttpClientFactoryPolicy[] _policies;

        public DefaultHttpClientFactory()
            : this(Array.Empty<IHttpClientFactoryPolicy>())
        {
        }

        public DefaultHttpClientFactory(IEnumerable<IHttpClientFactoryPolicy> policies)
        {
            if (policies == null)
            {
                throw new ArgumentNullException(nameof(policies));
            }

            _policies = policies.OrderBy(p => p.Order).ToArray();

            ClientHandler = new HttpClientHandler();
        }

        public HttpMessageHandler ClientHandler { get; set; }

        public override HttpClient GetNamedClient(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var context = new Context(name) { MessageHandler = ClientHandler };
            for (var i = 0; i < _policies.Length; i++)
            {
                _policies[i].Apply(context);
            }

            // There's other things we can set on the HTTP Client like the timeout and default headers.
            // Just using BaseAddress now to show what that does to the design.
            return new HttpClient(context.MessageHandler)
            {
                BaseAddress = context.BaseAddress,
            };
        }

        private class Context : HttpClientFactoryContext
        {
            public Context(string clientName)
            {
                ClientName = clientName;
            }

            public override Uri BaseAddress { get; set; }

            public override string ClientName { get; }

            public override HttpMessageHandler MessageHandler { get; set; }

            public override void PrependMessageHandler(DelegatingHandler messageHandler)
            {
                if (messageHandler == null)
                {
                    throw new ArgumentNullException(nameof(messageHandler));
                }

                messageHandler.InnerHandler = MessageHandler;
                MessageHandler = messageHandler;
            }
        }
    }
}
