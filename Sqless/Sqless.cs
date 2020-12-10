using Sqless.Request;
using Sqless.SqlBuilder;
using HTools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Sqless.Test")]
namespace Sqless
{
    /// <summary>
    /// Sqless
    /// </summary>
    public class Sqless : IDisposable
    {
        public SqlConnection Connection { get; }

        public Sqless(SqlessConfig config)
        {
            SqlessConfig = config;
            config.CheckConfigValid();

            Connection = new SqlConnection(config.SqlConStr);
        }
        public SqlessConfig SqlessConfig { get; }

        public async Task OpenSqlConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Closed || Connection.State == System.Data.ConnectionState.Broken)
            {
                await Connection.OpenAsync();
            }
        }

        public async Task<int> Insert(SqlessEditRequest request)
        {
            await OpenSqlConnection();
            SqlessInsertSqlBuilder producer = new SqlessInsertSqlBuilder(this, request);
            return await producer.ExecuteNonQueryAsync();
        }

        public async Task<int> Delete(SqlessDeleteRequest request)
        {
            await OpenSqlConnection();
            SqlessDeleteSqlBuilder producer = new SqlessDeleteSqlBuilder(this, request);
            return await producer.ExecuteNonQueryAsync();
        }

        public async Task<int> Update(SqlessEditRequest request)
        {
            await OpenSqlConnection();
            SqlessUpdateSqlBuilder producer = new SqlessUpdateSqlBuilder(this, request);
            return await producer.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 如果不存在，返回 default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<T> SelectFirstOrDefault<T>(SqlessSelectRequest request)
        {
            await OpenSqlConnection();
            SqlessSelectSqlBuilder selectSqlProducer = new SqlessSelectSqlBuilder(this, request);
            return await selectSqlProducer.ExecuteScalarAsync<T>(true);
        }

        public async Task<T> SelectFirst<T>(SqlessSelectRequest request)
        {
            await OpenSqlConnection();
            SqlessSelectSqlBuilder selectSqlProducer = new SqlessSelectSqlBuilder(this, request);
            return await selectSqlProducer.ExecuteScalarAsync<T>(false);
        }

        public async Task<List<dynamic>> Select(SqlessSelectRequest request)
        {
            await OpenSqlConnection();
            SqlessSelectSqlBuilder selectSqlProducer = new SqlessSelectSqlBuilder(this, request);
            using var dataTable = await selectSqlProducer.ExecuteTableAsync();
            return dataTable.ToList();
        }

        public async Task<List<T>> Select<T>(SqlessSelectRequest request) where T : class, new()
        {
            await OpenSqlConnection();
            SqlessSelectSqlBuilder selectSqlProducer = new SqlessSelectSqlBuilder(this, request);
            using var dataTable = await selectSqlProducer.ExecuteTableAsync();
            return dataTable.ToList<T>();
        }

        public async Task<int> Count(SqlessCountRequest request)
        {
            await OpenSqlConnection();
            SqlessCountSqlBuilder selectSqlProducer = new SqlessCountSqlBuilder(this, request);
            return await selectSqlProducer.ExecuteScalarAsync<int>();
        }

        public async Task<int> Upsert(SqlessEditRequest request)
        {
            using var sqless = new Sqless(SqlessConfig.GetAllowUnspecifiedConfig(this.SqlessConfig.SqlConStr));
            var count = await sqless.Count(new SqlessCountRequest()
            {
                Table = request.Table,
                Queries = request.Queries.Select(q => q.DeepClone()).ToList(),
            });

            if (count > 0)
            {
                return await Update(request);
            }
            else
            {
                return await Insert(request);
            }
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
