using System;
using Microsoft.Extensions.Options;
using Refit;

namespace Kickr.Refit
{

	public class RestClient<TClient> : IRestClient<TClient>
	{
		private RefitSettings _settings;
        private IOptionsMonitor<RestOptions> _optionsMonitor;

        public TClient Client { get; set; }

        public RestClient(IOptionsMonitor<RestOptions> optionsMonitor, HttpClientPipelineBuilder factory, IServiceProvider provider)
		{
            _optionsMonitor = optionsMonitor;
			_settings = new RefitSettings();
            _settings.HttpMessageHandlerFactory = () => factory.Build(provider);

            Client = RestService.For<TClient>(_optionsMonitor.Get(nameof(TClient)).Uri, 
                                              _settings);
		}

		public TClient GetClient(string url)
		{
			return RestService.For<TClient>(url, _settings);
		}
	}
}
