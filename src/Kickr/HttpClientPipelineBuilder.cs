using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr
{
	public delegate string UriKeyGenerator(Uri uri);

	public class HttpClientPipelineBuilder
    {
		public IServiceCollection Services { get; private set; }
        private List<Type> _handlerPipeline;
        private UriKeyGenerator _keyGenerator;

        public HttpClientPipelineBuilder(IServiceCollection services)
        {
            Services = services;
            _handlerPipeline = new List<Type>();
            _keyGenerator = (uri) => uri.ToString();
            services.AddSingleton(_keyGenerator);
        }

        public void AddHandler<T>() where T : DelegatingHandler
        {
            _handlerPipeline.Add(typeof(T));
        }

        public HttpClientPipelineBuilder UseKeyGenerator(UriKeyGenerator generator)
        {
            _keyGenerator = generator;
            Services.AddSingleton(_keyGenerator);
            return this;
        }

        public HttpMessageHandler Build(IServiceProvider provider)
        {
            _handlerPipeline.Reverse();
            HttpMessageHandler handler = new HttpClientHandler();

            foreach(var handlerType in _handlerPipeline)
            {
                handler = (DelegatingHandler)ActivatorUtilities.CreateInstance(provider, handlerType, handler);
            }

            return handler;
        }
    }
}
