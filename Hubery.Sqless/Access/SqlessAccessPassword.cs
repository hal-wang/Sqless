using System.Threading.Tasks;

namespace Hubery.Sqless.Access
{
    public class SqlessAccessPassword : SqlessAccess
    {
        public string AccessAccountField { get; set; }
        public string AccessPasswordField { get; set; }

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
