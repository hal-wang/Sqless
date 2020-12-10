using HTools;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sqless.Api
{
    internal class SqlessRequestHelper
    {
        private readonly RequestBase _requestBase;
        private SqlessRequestHelper(string url, string controller = null)
        {
            _requestBase = new RequestBase(url ?? SqlessClient.BaseUrl, controller);
        }

        public async Task<HttpResponseMessage> Post(string funcName, object param = null) => await _requestBase.Post(funcName, param);


        internal static SqlessRequestHelper Sqless { get; } = new SqlessRequestHelper(SqlessClient.BaseUrl, nameof(Sqless));
    }
}
