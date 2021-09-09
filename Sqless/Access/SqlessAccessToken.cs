using Sqless.Request;

namespace Sqless.Access {
    /// <summary>
    /// Token 登录认证
    /// </summary>
    public class SqlessAccessToken : SqlessAccess {
        public string AccessTokenField { get; set; }
        public string AccessTokenTypeField { get; set; }

        protected override SqlessSelectRequest GetRequest(string[] strs) {
            var request = base.GetRequest(strs);
            request.Queries.Add(new Query.SqlessQuery() {
                Field = AccessTokenField,
                Type = Query.SqlessQueryType.Equal,
                Value = strs[0]
            });
            if (!string.IsNullOrEmpty(AccessTokenTypeField)) {
                request.Queries.Add(new Query.SqlessQuery() {
                    Field = AccessTokenTypeField,
                    Type = Query.SqlessQueryType.Equal,
                    Value = strs[1]
                });
            }
            return request;
        }
    }
}
