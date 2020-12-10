using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Sqless.Common;
using Demo.Sqless.Common.Models;
using Sqless.Request;
using Sqless.SqlBuilder;
using System;
using System.Threading.Tasks;

namespace Sqless.Test
{
    [TestClass]
    public class AccessAllInsertTest
    {
        private SqlessEditRequest InsertRequest
        {
            get
            {
                var request = new SqlessEditRequest()
                {
                    Table = Tables.User
                };
                request.LoadFromObject(new User
                {
                    Uid = "InsertTest_" + Guid.NewGuid().ToString("D"),
                    Name = DateTime.Now.ToString("hhmmss"),
                    Password = new Random().Next(100000, 999999).ToString()
                });
                return request;
            }
        }

        [TestMethod]
        public async Task GetSqlStrTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessInsertSqlBuilder producer = new SqlessInsertSqlBuilder(sqless, InsertRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "INSERT INTO [User] ([Uid],[Name],[Password],[Phone]) VALUES (@Uid,@Name,@Password,@Phone)");
        }

        [TestMethod]
        public async Task InsertTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Insert(InsertRequest);
            Assert.IsTrue(result > 0);
        }


        [TestMethod]
        public async Task InsertTest2()
        {
            var request = new SqlessEditRequest()
            {
                Table = Tables.Product
            };
            request.LoadFromObject(new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Name = new Random().Next(100000, 999999).ToString(),
                Price = new Random().Next(1, 100)
            });

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Insert(request);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task InsertTest3()
        {
            var request = new SqlessEditRequest()
            {
                Table = Tables.Order
            };
            request.LoadFromObject(new Order()
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = new Random().Next(1, 4).ToString(),
                Uid = new Random().Next(1, 5).ToString(),
                Status = 1,
                Time = DateTimeOffset.Now.ToUnixTimeSeconds()
            });

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Insert(request);
            Assert.IsTrue(result > 0);
        }
    }
}
