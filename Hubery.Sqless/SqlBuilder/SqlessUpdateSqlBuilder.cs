using Hubery.Sqless.Request;
using System;
using System.Text;

namespace Hubery.Sqless.SqlBuilder
{
    internal class SqlessUpdateSqlBuilder : SqlessEditSqlBuilder
    {
        public SqlessUpdateSqlBuilder(Sqless sqless, SqlessEditRequest request)
            : base(sqless, request)
        {

        }

        protected override string GetSqlStr()
        {
            base.GetSqlStr();

            StringBuilder result = new StringBuilder();

            SetOwnerId();
            GetUpdateStr(result);
            GetQueryStr(result);

            return result.ToString();
        }

        private void GetUpdateStr(StringBuilder stringBuilder)
        {
            if ((SqlessRequest as SqlessEditRequest).Fields.Count == 0)
            {
                throw new ArgumentException("updating, fields are required");
            }

            stringBuilder.Append("UPDATE ");
            GetQueryTable(stringBuilder);
            stringBuilder.Append(" SET ");

            foreach (var field in (SqlessRequest as SqlessEditRequest).Fields)
            {
                stringBuilder.Append($"[{field.Field}] = @{field.Field},");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(" ");
        }
    }
}
