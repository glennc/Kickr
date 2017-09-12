using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kickr.Mvc
{
    public class HttpClientNameAttribute : Attribute, IBindingSourceMetadata
    {
        public HttpClientNameAttribute(string clientName)
        {
            if (clientName == null)
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            ClientName = clientName;
            BindingSource = new HttpClientBindingSource(ClientName);
        }

        public BindingSource BindingSource { get; }

        public string ClientName { get; }
    }
}
