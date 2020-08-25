using Hubery.Sqless.Request;
using System;
using System.Linq;
using System.Text;

namespace Hubery.Sqless.SqlBuilder
{
    internal class SqlessInsertSqlBuilder : SqlessEditSqlBuilder
    {
        public SqlessInsertSqlBuilder(Sqless sqless, Request.SqlessEditRequest request)
            : base(sqless, request)
        {

        }

        protected override string GetSqlStr()
        {
            base.GetSqlStr();

            StringBuilder result = new StringBuilder();

            SetOwnerId();
            GetInsertStr(result);
            //GetQueryStr(result);

            return result.ToString();
        }

        private void GetInsertStr(StringBuilder stringBuilder)
        {
            if ((SqlessRequest as SqlessEditRequest).Fields.Count == 0)
            {
                throw new ArgumentException("inserting, fields are required");
            }

            stringBuilder.Append("INSERT INTO ");
            GetQueryTable(stringBuilder);

            stringBuilder.Append(" (");
            foreach (var field in (SqlessRequest as SqlessEditRequest).Fields)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(field.Field);
                stringBuilder.Append("],");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") ");

            stringBuilder.Append(" VALUES ");

            stringBuilder.Append(" (");
            foreach (var field in (SqlessRequest as SqlessEditRequest).Fields)
            {
                stringBuilder.Append("@");
                stringBuilder.Append(field.Field);
                stringBuilder.Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") ");
        }
    }
}
