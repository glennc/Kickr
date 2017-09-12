
namespace Kickr.Http
{
    public interface IHttpClientFactoryPolicy
    {
        int Order { get; }

        void Apply(HttpClientFactoryContext context);
    }
}
