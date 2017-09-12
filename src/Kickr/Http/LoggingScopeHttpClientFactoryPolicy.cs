using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kickr.Http
{
    internal class LoggingScopeHttpClientFactoryPolicy : AbstractDelegatingHandlerHttpClientFactoryPolicy
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingScopeHttpClientFactoryPolicy(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _loggerFactory = loggerFactory;
        }

        // Run as late as possible so we get to create a scope as large as possible.
        public override int Order => Int32.MaxValue - 1000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new MessageHandler(CreateLogger(context.ClientName));
        }

        private ILogger CreateLogger(string clientName)
        {
            return _loggerFactory.CreateLogger("Kickr.Http." + clientName);
        }

        private class MessageHandler : DelegatingHandler
        {
            private readonly ILogger _logger;

            public MessageHandler(ILogger logger)
            {
                _logger = logger;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                using (_logger.BeginScope(new RequestScope
                {
                    HttpMethod = request.Method,
                    RequestUri = request.RequestUri,
                }))
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }
        }

        private class RequestScope
        {
            public Uri RequestUri { get; set; }

            public HttpMethod HttpMethod { get; set; }
        }
    }
}
