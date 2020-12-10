using Demo.Sqless.Common;
using Sqless.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqless.Test
{
    [TestClass]
    public class AccessAllUpsertTest
    {
        [TestMethod]
        public async Task UpsertUpdateTest()
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

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Upsert(request);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task UpsertInsertTest()
        {
            var newUid = "UpsertTest_" + Guid.NewGuid().ToString();
            var request = new SqlessEditRequest()
            {
                Table = Tables.User,
                Queries = new List<Query.SqlessQuery>()
                {
                    new Query.SqlessQuery()
                    {
                        Field="Uid",
                        Value=newUid,
                        Type=Query.SqlessQueryType.Equal
                    }
                }
            };
            request.LoadFromObject(new
            {
                Uid = newUid,
                Name = DateTime.Now.ToString("hhmmss"),
                Password = new Random().Next(100000, 999999).ToString()
            });

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var result = await sqless.Upsert(request);
            Assert.IsTrue(result > 0);
        }
    }
}
