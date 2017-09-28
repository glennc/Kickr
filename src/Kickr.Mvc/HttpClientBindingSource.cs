using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kickr.Mvc
{
    public class HttpClientBindingSource : BindingSource
    {
        public HttpClientBindingSource(string clientName)
            : base(Special.Id, Special.DisplayName, isGreedy: true, isFromRequest: false)
        {
            if (clientName == null)
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            ClientName = clientName;
        }

        public string ClientName { get; }
    }
}
