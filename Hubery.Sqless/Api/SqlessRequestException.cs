using System.Net.Http;
using System.Threading.Tasks;

namespace Hubery.Sqless.Api
{
    public class SqlessRequestException : HttpRequestException
    {
        public static async Task<SqlessRequestException> New(HttpResponseMessage res)
        {
            return new SqlessRequestException(res, await res.Content.ReadAsStringAsync());
        }

        private SqlessRequestException(HttpResponseMessage res, string message)
            : base(message)
        {
            HttpResponseMessage = res;
            HResult = (int)res.StatusCode;
        }

        public HttpResponseMessage HttpResponseMessage { get; private set; }
    }
}
