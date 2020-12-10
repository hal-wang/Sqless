using Sqless.Auth;
using Sqless.Query;
using Sqless.Request;
using HTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sqless.SqlBuilder
{
    public abstract class SqlessSqlBuilder
    {
        protected Sqless Sqless { get; }
        protected SqlessRequest SqlessRequest { get; }
        protected SqlessAuth Auth { get; }

        /// <summary>
        /// （insure connection is opened）
        /// </summary>
        /// <param name="sqless">sqless instance</param>
        /// <param name="request">sqless request</param>
        /// <param name="auth">readable or writable</param>
        protected SqlessSqlBuilder(Sqless sqless, SqlessRequest request, SqlessAuth auth)
        {
            Sqless = sqless;
            SqlessRequest = request;
            Auth = auth;
        }

        #region auth
        protected virtual List<string> GetAuthTables() => new List<string>() { SqlessRequest.Table };
        protected virtual List<SqlessField> GetAuthFields() => new List<SqlessField>();
        protected virtual List<string> GetAllTables() => new List<string>() { SqlessRequest.Table };
        protected virtual List<SqlessField> GetAllFields() => SqlessRequest.Queries.Select(q => new SqlessField() { Table = string.IsNullOrEmpty(q.Table) ? SqlessRequest.Table : q.Table, Field = q.Field }).ToList();

        protected bool IsTableReadable(string table)
        {
            var freeAuth = Sqless.SqlessConfig.FreeAuths.Where(fa => string.Equals(fa.Table, table, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (freeAuth != default)
            {
                return freeAuth.Readable;
            }

            var ownerAuth = Sqless.SqlessConfig.OwnerAuths.Where(item => string.Equals(item.Table, table, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (ownerAuth != default)
            {
                return ownerAuth.Readable;
            }

            return Sqless.SqlessConfig.IsUnspecifiedFreeReadable;
        }

        protected bool IsTableWritable(string table)
        {
            var freeAuth = Sqless.SqlessConfig.FreeAuths.Where(fa => string.Equals(fa.Table, table, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (freeAuth != default)
            {
                return freeAuth.Writable;
            }

            var ownerAuth = Sqless.SqlessConfig.OwnerAuths.Where(item => string.Equals(item.Table, table, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (ownerAuth != default)
            {
                return ownerAuth.Writable;
            }

            return Sqless.SqlessConfig.IsUnspecifiedFreeWritable;
        }

        protected bool IsTablesReadable() => GetAuthTables().Where(table => !IsTableReadable(table)).Count() == 0;
        protected bool IsTablesWritable() => GetAuthTables().Where(table => !IsTableWritable(table)).Count() == 0;

        protected bool IsFieldReadable(SqlessField sqlessField)
        {
            var fieldAuth = Sqless.SqlessConfig.FieldAuths.Where(item => (string.IsNullOrEmpty(item.Table) || string.IsNullOrEmpty(sqlessField.Table) || string.Equals(item.Table, sqlessField.Table, StringComparison.InvariantCultureIgnoreCase)) && string.Equals(item.Field, sqlessField.Field, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (fieldAuth == default)
            {
                return Sqless.SqlessConfig.IsUnspecifiedFieldReadable;
            }
            else
            {
                return fieldAuth.Readable;
            }
        }

        protected bool IsFieldWritable(SqlessField sqlessField)
        {
            var fieldAuth = Sqless.SqlessConfig.FieldAuths.Where(item => (string.IsNullOrEmpty(item.Table) || string.IsNullOrEmpty(sqlessField.Table) || string.Equals(item.Table, sqlessField.Table, StringComparison.InvariantCultureIgnoreCase)) && string.Equals(item.Field, sqlessField.Field, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (fieldAuth == default)
            {
                return Sqless.SqlessConfig.IsUnspecifiedFieldWritable;
            }
            else
            {
                return fieldAuth.Writable;
            }
        }

        protected bool IsFieldsReadable() => GetAuthFields().Where(item => !IsFieldReadable(item)).Count() == 0;
        protected bool IsFieldsWritable() => GetAuthFields().Where(item => !IsFieldWritable(item)).Count() == 0;


        #endregion

        #region Query
        protected void GetQueryStr(StringBuilder stringBuilder)
        {
            var authTables = GetAuthTables().Where(item => Sqless.SqlessConfig.OwnerAuths.Where(auth => string.Equals(auth.Table, item, StringComparison.InvariantCultureIgnoreCase)).Count() > 0).ToList();
            if (SqlessRequest.Queries.Count == 0 && authTables.Count == 0)
            {
                return;
            }

            stringBuilder.Append($" WHERE ");
            int index = 0;
            foreach (var query in SqlessRequest.Queries)
            {
                if (!string.IsNullOrEmpty(query.Table))
                {
                    stringBuilder.Append($" [{query.Table}].");
                }
                stringBuilder.Append($"[{query.Field}] ");
                stringBuilder.Append(QueryToSymble(query));
                if (ExistValue(query.Type))
                {
                    stringBuilder.Append($" @Query{index}");
                }
                stringBuilder.Append(" AND ");
                index++;
            }
            if (Sqless.SqlessConfig.OwnerAuths.Count == 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 4, 4);
            }
            else
            {
                foreach (var authTable in authTables)
                {
                    var auth = Sqless.SqlessConfig.OwnerAuths.Where(auth => string.Equals(auth.Table, authTable, StringComparison.InvariantCultureIgnoreCase)).First();

                    stringBuilder.Append($"[{auth.Table}].[{auth.Field}] ");
                    stringBuilder.Append($"= '{Sqless.SqlessConfig.AuthUid}'");
                    stringBuilder.Append(" AND ");
                }
                if (authTables.Count > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 4, 4);
                }
            }
        }

        protected void GetQueryTable(StringBuilder stringBuilder) => stringBuilder.Append($" [{SqlessRequest.Table}]");

        /// <summary>
        /// only queries params, if more, such as insert params, you must override and concat base.GetCmdParams()
        /// </summary>
        /// <returns></returns>
        protected virtual List<SqlParameter> GetCmdParams()
        {
            var result = new List<SqlParameter>();
            for (var i = 0; i < SqlessRequest.Queries.Count; i++)
            {
                var query = SqlessRequest.Queries[i];

                if (!ExistValue(query.Type))
                {
                    continue;
                }

                result.Add(new SqlParameter($"@Query{i}", QueryToValue(query)));
            }
            return result;
        }

        private bool ExistValue(SqlessQueryType type) => type != SqlessQueryType.Null && type != SqlessQueryType.NotNull;

        protected object QueryToValue(SqlessQuery query)
        {
            if (query.Value == null)
            {
                return DBNull.Value;
            }

            return query.Type switch
            {
                SqlessQueryType.Contain => $"%{query.Value}%",
                SqlessQueryType.StartWith => $"{query.Value}%",
                SqlessQueryType.EndWith => $"%{query.Value}",
                SqlessQueryType.LessEqual => $"{query.Value}",
                SqlessQueryType.GreaterEqual => $"{query.Value}",
                SqlessQueryType.LessThan => $"{query.Value}",
                SqlessQueryType.GreaterThan => $"{query.Value}",
                SqlessQueryType.Equal => $"{query.Value}",
                SqlessQueryType.NotEqual => $"{query.Value}",
                SqlessQueryType.Null => $"",
                SqlessQueryType.NotNull => $"",
                _ => throw new NotSupportedException("SqlessQueryType")
            };
        }


        protected string QueryToSymble(SqlessQuery query) =>
            query.Type switch
            {
                SqlessQueryType.Contain => "LIKE",
                SqlessQueryType.StartWith => "LIKE",
                SqlessQueryType.EndWith => "LIKE",
                SqlessQueryType.LessEqual => "<=",
                SqlessQueryType.GreaterEqual => ">=",
                SqlessQueryType.LessThan => "<",
                SqlessQueryType.GreaterThan => ">",
                SqlessQueryType.Equal => "=",
                SqlessQueryType.NotEqual => "!=",
                SqlessQueryType.Null => "IS NULL",
                SqlessQueryType.NotNull => "IS NOT NULL",
                _ => throw new NotSupportedException("SqlessQueryType")
            };
        #endregion

        private bool IsCharactersIllegal(string str) => Regex.IsMatch(str, "^\\w{1,128}$");

        protected virtual string GetSqlStr()
        {
            if (GetAllTables().Where(str => !IsCharactersIllegal(str)).Count() > 0 || GetAllFields().Where(f => !IsCharactersIllegal(f.Field)).Count() > 0)
            {
                throw new FormatException("illegal character");
            }

            if (Auth.Readable)
            {
                if (!IsTablesReadable())
                {
                    throw new UnauthorizedAccessException("can't read table");
                }
                if (!IsFieldsReadable())
                {
                    throw new UnauthorizedAccessException("can't read fields");
                }
            }
            if (Auth.Writable)
            {
                if (!IsTablesWritable())
                {
                    throw new UnauthorizedAccessException("can't write table");
                }
                if (!IsFieldsWritable())
                {
                    throw new UnauthorizedAccessException("can't write fields");
                }
            }

            return "";
        }

        /// <summary>
        /// 单元测试使用
        /// </summary>
        /// <returns></returns>
        internal async Task<string> GetSqlStrTest()
        {
            await Sqless.OpenSqlConnection();
            return GetSqlStr();
        }

        private SqlCommand GetSqlCommand()
        {
            SqlCommand sqlcmd = new SqlCommand(GetSqlStr(), Sqless.Connection);
            var cmdParams = GetCmdParams();
            if (cmdParams.Count > 0)
            {
                sqlcmd.Parameters.AddRange(cmdParams.ToArray());
            }
            return sqlcmd;
        }

        internal async Task<DataTable> ExecuteTableAsync()
        {
            var result = new DataTable();

            await TaskExtend.Run(() =>
            {
                using SqlCommand sqlcmd = GetSqlCommand();
                using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlcmd);
                sqlDataAdapter.Fill(result);
            });

            return result;
        }

        internal async Task<T> ExecuteScalarAsync<T>(bool defaultIfNull = false)
        {
            using SqlCommand sqlcmd = GetSqlCommand();
            var result = await sqlcmd.ExecuteScalarAsync();
            if (result == null)
            {
                if (defaultIfNull)
                {
                    return default;
                }
                else
                {
                    throw new SqlNullValueException();
                }
            }
            else
            {
                return (T)result;
            }
        }

        internal async Task<int> ExecuteNonQueryAsync()
        {
            using SqlCommand sqlcmd = GetSqlCommand();
            return await sqlcmd.ExecuteNonQueryAsync();
        }
    }
}
