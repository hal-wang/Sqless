using System.Net.Http;

namespace Sqless.Api {
    public class SqlessRequestException : HttpRequestException
    {
        public SqlessRequestException(HttpResponseMessage res, string message)
            : base(message)
        {
            HttpResponseMessage = res;
            HResult = (int)res.StatusCode;
        }

        public HttpResponseMessage HttpResponseMessage { get; private set; }
    }
}
