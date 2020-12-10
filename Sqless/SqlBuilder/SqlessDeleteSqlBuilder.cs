using Sqless.Auth;
using System.Text;

namespace Sqless.SqlBuilder
{
    internal class SqlessDeleteSqlBuilder : SqlessSqlBuilder
    {
        public SqlessDeleteSqlBuilder(Sqless sqless, Request.SqlessDeleteRequest request)
            : base(sqless, request, new SqlessAuth() { Writable = true, Readable = false })
        {

        }

        protected override string GetSqlStr()
        {
            base.GetSqlStr();

            StringBuilder result = new StringBuilder();

            GetDeleteStr(result);
            GetQueryStr(result);

            return result.ToString();
        }

        private void GetDeleteStr(StringBuilder stringBuilder)
        {
            stringBuilder.Append("DELETE FROM ");
            GetQueryTable(stringBuilder);
            stringBuilder.Append(" ");
        }
    }
}
