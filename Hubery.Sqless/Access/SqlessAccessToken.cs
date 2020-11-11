using System.Threading.Tasks;

namespace Hubery.Sqless.Access
{
    /// <summary>
    /// Token 登录认证
    /// </summary>
    public class SqlessAccessToken : SqlessAccess
    {
        public string AccessTokenField { get; set; }
        public string AccessTokenTypeField { get; set; }

        /// <summary>
        /// 根据 Token 获取用户ID
        /// </summary>
        /// <param name="strs">第一个参数为Token，第二个参数可选，比如根据Platform有不同Token</param>
        /// <returns></returns>
        public override async Task<string> GetUid(params string[] strs)
        {
            var request = SelectRequestBase;
            request.Queries.Add(new Query.SqlessQuery()
            {
                Field = AccessTokenField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[0]
            });

            if (!string.IsNullOrEmpty(AccessTokenTypeField))
            {
                request.Queries.Add(new Query.SqlessQuery()
                {
                    Field = AccessTokenTypeField,
                    Type = Query.SqlessQueryType.Equal,
                    Value = strs[1]
                });
            }

            using Sqless sqless = new Sqless(SqlessConfig);
            return await sqless.SelectFirst<string>(request);
        }
    }
}
