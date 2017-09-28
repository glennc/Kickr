using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kickr.Mvc
{
    internal class HttpClientModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType== typeof(HttpClient))
            {
                var clientName = context.BindingInfo.BindingSource is HttpClientBindingSource source ? source.ClientName : HttpClientFactory.DefaultClientName;
                return new HttpClientModelBinder(clientName);
            }

            return null;
        }
    }
}
