using Hubery.Sqless.Auth;
using Hubery.Sqless.Query;
using Hubery.Sqless.Request;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hubery.Sqless.SqlBuilder
{
    public class SqlessCountSqlBuilder : SqlessSqlBuilder
    {
        public SqlessCountSqlBuilder(Sqless sqless, SqlessCountRequest request)
            : base(sqless, request, new SqlessAuth() { Writable = false, Readable = true })
        {

        }

        protected override string GetSqlStr()
        {
            base.GetSqlStr();

            StringBuilder result = new StringBuilder();

            GetCountStr(result);
            GetQueryStr(result);

            return result.ToString();
        }

        protected override List<SqlessField> GetAuthFields()
        {
            var result = base.GetAuthFields();
            AddFieldToList(result);
            return result;
        }

        protected override List<SqlessField> GetAllFields()
        {
            var result = base.GetAllFields();
            AddFieldToList(result);
            return result;
        }

        private void AddFieldToList(List<SqlessField> fields)
        {
            var request = SqlessRequest as SqlessCountRequest;
            if (string.IsNullOrEmpty(request.Field))
            {
                return;
            }

            fields.Add(new SqlessField()
            {
                Table = request.Table,
                Field = request.Field
            });
        }

        private void GetCountStr(StringBuilder stringBuilder)
        {
            var request = SqlessRequest as SqlessCountRequest;
            if (request.Distinct && string.IsNullOrEmpty(request.Field))
            {
                throw new NoNullAllowedException("if distinct count, the field is no null or empty allowed");
            }

            stringBuilder.Append("SELECT COUNT(");
            if (request.Distinct)
            {
                stringBuilder.Append("DISTINCT ");
            }
            if (!string.IsNullOrEmpty(request.Field))
            {
                stringBuilder.Append($"[{request.Field}]");
            }
            else
            {
                stringBuilder.Append("*");
            }
            stringBuilder.Append(")");
            stringBuilder.Append(" FROM ");
            GetQueryTable(stringBuilder);
            stringBuilder.Append(" ");
        }
    }
}
