using System;
namespace Kickr.Refit
{
	public interface IRestClient<TClient>
	{
		TClient Client { get; set; }

		TClient GetClient(string url);
	}
}
