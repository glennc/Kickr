using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kickr
{
    public class HttpClientPipelineBuilder
    {
        public IServiceCollection Services { get; private set; }
        private List<Type> _handlerPipeline;

        public HttpMessageHandler BaseHandler { get; protected set; }

        public HttpClientPipelineBuilder(IServiceCollection services)
        {
            Services = services;
            _handlerPipeline = new List<Type>();
            BaseHandler = new HttpClientHandler();
        }

        public void AddHandler<T>() where T : DelegatingHandler
        {
            _handlerPipeline.Add(typeof(T));
        }

        public HttpMessageHandler Build(IServiceProvider provider)
        {
            _handlerPipeline.Reverse();
            var handler = BaseHandler;
            foreach (var handlerType in _handlerPipeline)
            {
                var current = (DelegatingHandler)ActivatorUtilities.CreateInstance(provider, handlerType);
                current.InnerHandler = handler;
                handler = current;
            }

            return handler;
        }
    }
}
