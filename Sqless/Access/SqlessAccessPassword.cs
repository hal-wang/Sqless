using System.Threading.Tasks;

namespace Sqless.Access
{
    /// <summary>
    /// 账号+密码 登录认证
    /// </summary>
    public class SqlessAccessPassword : SqlessAccess
    {
        public string AccessAccountField { get; set; }
        public string AccessPasswordField { get; set; }

        /// <summary>
        /// 根据账号密码获取用户ID
        /// </summary>
        /// <param name="strs">账号，密码</param>
        /// <returns></returns>
        public override async Task<string> GetUid(params string[] strs)
        {
            var request = SelectRequestBase;
            request.Queries.Add(new Query.SqlessQuery()
            {
                Field = AccessAccountField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[0]
            });
            request.Queries.Add(new Query.SqlessQuery()
            {
                Field = AccessPasswordField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[1]
            });

            using Sqless sqless = new Sqless(SqlessConfig);
            return await sqless.SelectFirst<string>(request);
        }
    }
}
