using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Refit;

namespace refitsample.Services
{
    public interface IConferencePlannerApi
    {
        [Get("/api/sessions")]
        Task<IEnumerable<JObject>> GetSessionsAsync();
    }
}
