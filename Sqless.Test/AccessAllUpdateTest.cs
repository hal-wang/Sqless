using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sqless.Demo.Common;
using Sqless.Request;
using Sqless.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqless.Test
{
    [TestClass]
    public class AccessAllUpdateTest
    {
        private SqlessEditRequest UpdateRequest
        {
            get
            {
                var request = new SqlessEditRequest()
                {
                    Table = Tables.User,
                    Queries = new List<Query.SqlessQuery>()
                    {
                        new Query.SqlessQuery()
                        {
                            Field="Uid",
                            Value="1",
                            Type=Query.SqlessQueryType.Equal
                        }
                    }
                };
                request.LoadFromObject(new
                {
                    Phone = DateTime.Now.ToString("hhmmss"),
                    Password = new Random().Next(100000, 999999).ToString()
                });
                return request;
            }
        }

        [TestMethod]
        public async Task GetSqlStrTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessUpdateSqlBuilder producer = new SqlessUpdateSqlBuilder(sqless, UpdateRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "UPDATE [User] SET [Phone] = @Phone,[Password] = @Password WHERE [Uid] = @Query0");
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Update(UpdateRequest);
            Assert.IsTrue(result > 0);
        }
    }
}
