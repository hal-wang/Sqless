using Sqless.Request;

namespace Sqless.Access {
    /// <summary>
    /// 账号+密码 登录认证
    /// </summary>
    public class SqlessAccessPassword : SqlessAccess {
        public string AccessAccountField { get; set; }
        public string AccessPasswordField { get; set; }

        protected override SqlessSelectRequest GetRequest(string[] strs) {
            var request = base.GetRequest(strs);
            request.Queries.Add(new Query.SqlessQuery() {
                Field = AccessAccountField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[0]
            });
            request.Queries.Add(new Query.SqlessQuery() {
                Field = AccessPasswordField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[1]
            });
            return request;
        }
    }
}
