using HTools;
using Sqless.Auth;
using Sqless.Query;
using Sqless.Request;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace Sqless.SqlBuilder
{
    internal class SqlessEditSqlBuilder : SqlessSqlBuilder
    {
        public SqlessEditSqlBuilder(Sqless sqless, Request.SqlessEditRequest request)
            : base(sqless, request, new SqlessAuth() { Writable = true, Readable = false })
        {

        }

        protected override List<SqlessField> GetAuthFields() =>
            base.GetAuthFields()
            .Concat((SqlessRequest as SqlessEditRequest).Fields.Select(item => new SqlessField() { Table = SqlessRequest.Table, Field = item.Field }).ToList())
            .ToList();

        protected void SetOwnerId()
        {
            var request = SqlessRequest as SqlessEditRequest;

            var ownerOath = Sqless.SqlessConfig.OwnerAuths.Where(oa => string.Equals(oa.Table, SqlessRequest.Table, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (ownerOath == default)
            {
                return;
            }

            var field = request.Fields.Where(f => string.Equals(f.Field, ownerOath.Field, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (field == default)
            {
                request.Fields.Add(new SqlessEditField()
                {
                    Field = ownerOath.Field,
                    Value = Sqless.SqlessConfig.AuthUid
                });
            }
            else
            {
                field.Value = Sqless.SqlessConfig.AuthUid;
            }
        }

        protected override List<SqlParameter> GetCmdParams()
        {
            var baseParams = base.GetCmdParams();

            var result = new List<SqlParameter>();
            foreach (var field in (SqlessRequest as SqlessEditRequest).Fields)
            {
                result.Add(GetEditSqlParameter(field));
            }

            return result.Concat(baseParams).ToList();
        }

        private SqlParameter GetEditSqlParameter(SqlessEditField field)
        {
            object value;
            if (field.Value == null)
            {
                value = DBNull.Value;
            }
            else if (field.Value is JsonElement je)
            {
                value = je.GetValue(TypeConverter.DbTypeToType(field.Type));
            }
            else
            {
                value = Convert.ChangeType(field.Value, TypeConverter.DbTypeToType(field.Type));
            }

            var result = new SqlParameter($"@{field.Field}", value)
            {
                DbType = field.Type
            };

            return result;
        }
    }
}
