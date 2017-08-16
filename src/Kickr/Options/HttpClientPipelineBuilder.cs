using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr.Options
{
    public class HttpClientPipelineBuilder
    {
        public IServiceCollection Services { get; private set; }
        private List<Type> _handlerPipeline;

        public HttpClientPipelineBuilder(IServiceCollection services)
        {
            Services = services;
            _handlerPipeline = new List<Type>();
        }

        public void AddHandler<T>() where T : DelegatingHandler
        {
            _handlerPipeline.Add(typeof(T));
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
