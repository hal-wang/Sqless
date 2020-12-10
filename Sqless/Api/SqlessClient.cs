using Hubery.Tools;
using Sqless.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqless.Api
{
    public static class SqlessClient
    {
        public static string BaseUrl { get; set; }

        private static RequestBase RequestBase => new RequestBase(BaseUrl, nameof(Sqless));

        public static async Task<T> Post<T>(SqlessRequest request, SqlessApiType sqlessApiType)
        {
            var res = await RequestBase.Post(sqlessApiType.ToString(), request);
            if (!res.IsSuccessStatusCode)
            {
                throw await SqlessRequestException.New(res);
            }
            return await res.GetContent<T>();
        }

        [SqlessApiAction]
        public static async Task<List<T>> Select<T>(SqlessSelectRequest request) => await Post<List<T>>(request, SqlessApiType.Select);
        [SqlessApiAction]
        public static async Task<T> SelectFirst<T>(SqlessSelectRequest request) => await Post<T>(request, SqlessApiType.SelectFirst);
        [SqlessApiAction]
        public static async Task<T> SelectFirstOrDefault<T>(SqlessSelectRequest request) => await Post<T>(request, SqlessApiType.SelectFirstOrDefault);
        [SqlessApiAction]
        public static async Task<int> Delete(SqlessDeleteRequest request) => await Post<int>(request, SqlessApiType.Delete);
        [SqlessApiAction]
        public static async Task<int> Insert(SqlessEditRequest request) => await Post<int>(request, SqlessApiType.Insert);
        [SqlessApiAction]
        public static async Task<int> Update(SqlessEditRequest request) => await Post<int>(request, SqlessApiType.Update);
        [SqlessApiAction]
        public static async Task<int> Upsert(SqlessEditRequest request) => await Post<int>(request, SqlessApiType.Upsert);
    }
}
