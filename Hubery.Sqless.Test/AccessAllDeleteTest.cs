using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hubery.Sqless.Demo.Common;
using Hubery.Sqless.Request;
using Hubery.Sqless.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubery.Sqless.Test
{
    [TestClass]
    public class AccessAllDeleteTest
    {
        private async Task<string> Insert()
        {
            var id = "DeleteTest_" + Guid.NewGuid().ToString("D");
            var request = new SqlessEditRequest()
            {
                Table = Tables.User
            };
            request.LoadFromObject(new User
            {
                Uid = id,
                Name = DateTime.Now.ToString("hhmmss"),
                Password = new Random().Next(100000, 999999).ToString()
            });

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Insert(request);
            Assert.IsTrue(result > 0);
            return id;
        }

        private SqlessDeleteRequest GetDeleteSqlTestRequest(string id)
        {
            return new SqlessDeleteRequest()
            {
                Table = Tables.User,
                Queries = new List<Query.SqlessQuery>()
                {
                    new Query.SqlessQuery()
                    {
                        Field="Uid",
                        Type=Query.SqlessQueryType.Equal,
                        Value=id
                    }
                }
            };
        }

        [TestMethod]
        public async Task DeleteSqlTest()
        {
            var id = await Insert();

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessDeleteSqlBuilder producer = new SqlessDeleteSqlBuilder(sqless, GetDeleteSqlTestRequest(id));
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "DELETE FROM [User] WHERE [Uid] = @Query0");
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            var id = await Insert();

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Delete(GetDeleteSqlTestRequest(id));
            Assert.IsTrue(result > 0);
        }
    }
}
