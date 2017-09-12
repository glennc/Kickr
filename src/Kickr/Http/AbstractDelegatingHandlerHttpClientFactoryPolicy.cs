using System;
using System.Net.Http;

namespace Kickr.Http
{
    public abstract class AbstractDelegatingHandlerHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        public abstract int Order { get; }

        protected abstract DelegatingHandler CreateHandler(HttpClientFactoryContext context);

        public void Apply(HttpClientFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var handler = CreateHandler(context);
            handler.InnerHandler = context.MessageHandler;
            context.MessageHandler = handler;
        }
    }
}
