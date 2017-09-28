using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Kickr.Mvc
{
    internal class HttpClientModelBinder : IModelBinder
    {
        public HttpClientModelBinder(string clientName)
        {
            if (clientName == null)
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            ClientName = clientName;
        }

        public string ClientName { get; }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var clientFactory = bindingContext.HttpContext.RequestServices.GetRequiredService<HttpClientFactory>();
            var client = clientFactory.GetNamedClient(ClientName);

            bindingContext.Result = ModelBindingResult.Success(client);
            bindingContext.ValidationState.Add(client, new ValidationStateEntry()
            {
                SuppressValidation = true,
            });

            return Task.CompletedTask;
        }
    }
}
