using System;
using Kickr.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KickrMvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddKickr(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Add(new HttpClientModelBinderProvider());
            });

            return builder;
        }

        public static IMvcBuilder AddKickr(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Add(new HttpClientModelBinderProvider());
            });

            return builder;
        }
    }
}
