using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Sqless.Common;
using Demo.Sqless.Common.Models;
using Sqless.Query;
using Sqless.Request;
using Sqless.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqless.Test
{
    [TestClass]
    public class OwnerAuthTest
    {
        [TestMethod]
        public async Task AccessSelectSqlTest()
        {
            var request = new SqlessSelectRequest()
            {
                Table = Tables.User,
                PageSize = 2,
                Fields = new List<Query.SqlessField>
                {
                    new Query.SqlessField(){Field="Name"}
                }
            };

            using Sqless sqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, request);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT TOP 2 [Name] FROM [User] WHERE [User].[Uid] = '1'");
        }

        [TestMethod]
        public async Task AccessSelectTest()
        {
            var request = new SqlessSelectRequest()
            {
                Table = Tables.User,
                PageSize = 2,
                Fields = new List<Query.SqlessField>
                {
                    new Query.SqlessField(){Field="Name"}
                }
            };

            using Sqless sqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            var result = await sqless.Select(request);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task NoAccessSelectTest()
        {
            var reqeust = new SqlessEditRequest()
            {
                Table = Tables.User,
                Fields = new List<SqlessEditField>()
                {
                    new SqlessEditField()
                    {
                        Field="Password",
                        Value="1",
                        Type=System.Data.DbType.String
                    }
                }
            };

            using Sqless sqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            try
            {
                var result = await sqless.Update(reqeust);
            }
            catch (UnauthorizedAccessException)
            {
                Assert.IsTrue(true);
                return;
            }

            Assert.IsTrue(false);
        }

        [TestMethod]
        public async Task UpdateOwnerFieldSqlTest()
        {
            var insertRequest = new SqlessEditRequest()
            {
                Table = Tables.User,
                Fields = new List<SqlessEditField>()
                {
                    new SqlessEditField()
                    {
                        Field="Uid",
                        Value="2",
                        Type=System.Data.DbType.String
                    }
                }
            };
            using Sqless insertSqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            SqlessInsertSqlBuilder producer = new SqlessInsertSqlBuilder(insertSqless, insertRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(insertRequest.Fields[0].Value, "1");
        }

        [TestMethod]
        public async Task UpdateOwnerFieldTest()
        {
            var updateRequest = new SqlessEditRequest()
            {
                Table = Tables.User,
                Fields = new List<SqlessEditField>()
                {
                    new SqlessEditField()
                    {
                        Field="Uid",
                        Value="2",
                        Type=System.Data.DbType.String
                    }
                }
            };
            using Sqless updateSqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            var updateResult = await updateSqless.Update(updateRequest);
            Assert.IsTrue(updateResult > 0);

            var selectRequest = new SqlessSelectRequest()
            {
                Table = Tables.User,
                Fields = new List<Query.SqlessField>()
                {
                    new SqlessField()    {     Field = "Uid"     }
                }
            };
            using Sqless selectSqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            var selectResult = await selectSqless.SelectFirstOrDefault<string>(selectRequest);
            Assert.AreEqual(selectResult, "1");
        }

        [TestMethod]
        public async Task UpdateOwnerField1Test()
        {
            var request = new SqlessEditRequest()
            {
                Table = Tables.User,
                Fields = new List<SqlessEditField>()
                {
                    new SqlessEditField()
                    {
                        Field="Name",
                        Value="1",
                        Type=System.Data.DbType.String
                    }
                }
            };

            using Sqless sqless = new Sqless(Global.GetOwnerAccessConfig("1"));
            var result = await sqless.Update(request);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task InsertTest()
        {
            var order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = "1",
                Status = 1,
                Time = DateTimeOffset.Now.ToUnixTimeSeconds(),
            };
            var request = new SqlessEditRequest()
            {
                Table = Tables.Order,
                AccessParams = new string[] { "AccessTestUid", "123456" },
            };
            request.LoadFromObject(order);

            var uid = await Global.PasswordAccessConfig.GetUid(request.AccessParams);
            using Sqless sqless = new Sqless(Global.GetOwnerAccessConfig(uid));
            var result = await sqless.Insert(request);
            Assert.IsTrue(result > 0);
        }
    }
}
