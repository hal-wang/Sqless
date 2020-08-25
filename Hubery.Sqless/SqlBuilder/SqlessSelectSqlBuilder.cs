using Hubery.Sqless.Auth;
using Hubery.Sqless.Query;
using Hubery.Sqless.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hubery.Sqless.SqlBuilder
{
    internal class SqlessSelectSqlBuilder : SqlessSqlBuilder
    {
        public SqlessSelectSqlBuilder(Sqless sqless, SqlessSelectRequest request)
            : base(sqless, request, new SqlessAuth() { Writable = false, Readable = true })
        {

        }

        protected override string GetSqlStr()
        {
            base.GetSqlStr();

            StringBuilder result = new StringBuilder();

            GetSelectStr(result);
            GetJoinStr(result);

            if ((SqlessRequest as SqlessSelectRequest).Joins.Count > 0
                && SqlessRequest.Queries.Where(q => string.IsNullOrEmpty(q.Table)).Count() > 0)
            {
                throw new ArgumentNullException("if join table, queries table is not allowed null or empty");
            }

            GetQueryStr(result);
            GetOrderStr(result);
            GetPageStr(result);

            return result.ToString();
        }

        private void GetSelectStr(StringBuilder stringBuilder)
        {
            var request = SqlessRequest as SqlessSelectRequest;
            stringBuilder.Append("SELECT ");
            if (request.PageSize > 0 && request.Orders.Count == 0) // exist pageSize but order
            {
                stringBuilder.Append(" TOP ");
                stringBuilder.Append(request.PageSize);
                stringBuilder.Append(" ");
            }

            if (request.Fields.Count() == 0)
            {
                throw new ArgumentException("There is no field");
            }

            if (request.Fields.GroupBy(f => f.Field).Where(g => g.Count() > 1).Count() > 0)
            {
                throw new ArgumentException("Field names cannot be the same");
            }

            foreach (var fieldItem in request.Fields)
            {
                if (!string.IsNullOrEmpty(fieldItem.Table))
                {
                    stringBuilder.Append($"[{fieldItem.Table}].");
                }
                stringBuilder.Append($"[{fieldItem.Field}],");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(" FROM ");
            GetQueryTable(stringBuilder);
        }

        private void GetJoinStr(StringBuilder stringBuilder)
        {
            foreach (var join in (SqlessRequest as SqlessSelectRequest).Joins)
            {
                stringBuilder.Append(" ");
                var joinStr = join.SqlessJoinType switch
                {
                    SqlessJoinType.FullJoin => "FULL JOIN",
                    SqlessJoinType.LeftJoin => "LEFT JOIN",
                    SqlessJoinType.InnerJoin => "INNER JOIN",
                    SqlessJoinType.RightJoin => "RIGHT JOIN",
                    _ => throw new NotSupportedException()
                };
                stringBuilder.Append(joinStr);

                stringBuilder.Append(" [");
                stringBuilder.Append(join.RightTable);
                stringBuilder.Append("] ON ");
                stringBuilder.Append($" [{join.LeftTable}].[{join.LeftField}] = [{join.RightTable}].[{join.RightField}] ");
            }
        }

        private void GetOrderStr(StringBuilder stringBuilder)
        {
            if ((SqlessRequest as SqlessSelectRequest).Orders.Count == 0)
            {
                return;
            }

            stringBuilder.Append(" ORDER BY ");
            foreach (var order in (SqlessRequest as SqlessSelectRequest).Orders)
            {
                stringBuilder.Append(string.IsNullOrEmpty(order.Table) ? $" [{order.Field}] " : $" [{order.Table}].[{order.Field}] ");
                stringBuilder.Append(order.Direction.ToString());
                stringBuilder.Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(" ");
        }

        private void GetPageStr(StringBuilder stringBuilder)
        {
            if ((SqlessRequest as SqlessSelectRequest).Orders.Count == 0) // paging query, must order first
            {
                return;
            }

            stringBuilder.Append(" OFFSET ");
            stringBuilder.Append((SqlessRequest as SqlessSelectRequest).PageIndex * (SqlessRequest as SqlessSelectRequest).PageSize);
            stringBuilder.Append(" ROWS ");

            if ((SqlessRequest as SqlessSelectRequest).PageSize > 0)
            {
                stringBuilder.Append(" FETCH NEXT ");
                stringBuilder.Append((SqlessRequest as SqlessSelectRequest).PageSize);
                stringBuilder.Append(" ROWS ONLY ");
            }
        }

        protected override List<string> GetAuthTables() =>
            base.GetAuthTables()
            .Concat((SqlessRequest as SqlessSelectRequest).Joins.Select(item => item.RightTable).ToList())
            .ToList();

        protected override List<SqlessField> GetAuthFields() =>
            base.GetAuthFields()
            .Concat((SqlessRequest as SqlessSelectRequest).Fields.ToList())
            .ToList();

        protected override List<string> GetAllTables() =>
            base.GetAllTables()
            .Concat((SqlessRequest as SqlessSelectRequest).Joins.Select(item => item.RightTable).ToList())
            .ToList();

        protected override List<SqlessField> GetAllFields() =>
            base.GetAllFields()
            .Concat((SqlessRequest as SqlessSelectRequest).Fields.Select(item => new SqlessField() { Table = string.IsNullOrEmpty(item.Table) ? SqlessRequest.Table : item.Table, Field = item.Field }).ToList())
            .Concat((SqlessRequest as SqlessSelectRequest).Orders.Select(item => new SqlessField() { Table = string.IsNullOrEmpty(item.Table) ? SqlessRequest.Table : item.Table, Field = item.Field }).ToList())
            .Concat((SqlessRequest as SqlessSelectRequest).Joins.Select(item => new SqlessField() { Table = string.IsNullOrEmpty(item.LeftTable) ? throw new NoNullAllowedException("join, not table null allowed") : item.LeftField, Field = item.LeftField }).ToList())
            .Concat((SqlessRequest as SqlessSelectRequest).Joins.Select(item => new SqlessField() { Table = string.IsNullOrEmpty(item.RightTable) ? throw new NoNullAllowedException("join, not table null allowed") : item.RightTable, Field = item.RightField }).ToList())
            .ToList();
    }
}
