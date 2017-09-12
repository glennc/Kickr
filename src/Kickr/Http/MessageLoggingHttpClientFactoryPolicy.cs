using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kickr.Http
{
    internal class MessageLoggingHttpClientFactoryPolicy : AbstractDelegatingHandlerHttpClientFactoryPolicy
    {
        private readonly ILoggerFactory _loggerFactory;

        public MessageLoggingHttpClientFactoryPolicy(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _loggerFactory = loggerFactory;
        }

        // Run as early as possible so we get to log as close as possible to the actual request.
        public override int Order => Int32.MinValue + 1000;

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

            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Sending {HttpMethod} request to {RequestUri}", request.Method, request.RequestUri);
                var response = await base.SendAsync(request, cancellationToken);
                _logger.LogInformation("Response was a {Status}", response.StatusCode);
                return response;
            }
        }
    }
}
