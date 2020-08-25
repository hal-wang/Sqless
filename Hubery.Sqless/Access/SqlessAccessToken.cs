using System.Threading.Tasks;

namespace Hubery.Sqless.Access
{
    public class SqlessAccessToken : SqlessAccess
    {
        public string AccessTokenField { get; set; }
        public string AccessTokenTypeField { get; set; }

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
