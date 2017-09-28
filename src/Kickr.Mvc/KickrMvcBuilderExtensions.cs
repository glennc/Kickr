using System;
using Kickr.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KickrMvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddKickrModelBinder(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddKickr();

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new HttpClientModelBinderProvider());
            });

            return builder;
        }

        public static IMvcBuilder AddKickrModelBinder(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddKickr();

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new HttpClientModelBinderProvider());
            });

            return builder;
        }
    }
}
