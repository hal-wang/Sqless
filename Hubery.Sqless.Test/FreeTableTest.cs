using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Hubery.Sqless.Demo.Common;
using Hubery.Sqless.Request;
using Hubery.Sqless.SqlBuilder;
using System;
using System.Threading.Tasks;

namespace Hubery.Sqless.Test
{
    [TestClass]
    public class FreeTableTest
    {
        private SqlessSelectRequest SelectRequest => new SqlessSelectRequest()
        {
            Table = Tables.Product,
            PageSize = 2,
            Fields = new System.Collections.Generic.List<Query.SqlessField>()
            {
                new Query.SqlessField(){Field="Name"},
                new Query.SqlessField(){Field="Price"},
            }
        };
        private SqlessConfig SelectConfig
        {
            get
            {
                var result = Global.DisallowedUnspecifiedConfig;
                result.FreeAuths.Add(new Auth.SqlessAuth()
                {
                    Readable = true,
                    Writable = false,
                    Table = Tables.Product
                });
                result.FieldAuths = new System.Collections.Generic.List<Auth.SqlessFieldAuth>()
                {
                    new Auth.SqlessFieldAuth(){Table=Tables.Product, Field="Name",Readable=true},
                    new Auth.SqlessFieldAuth(){Table=Tables.Product, Field="Price",Readable=true}
                };

                return result;
            }
        }

        [TestMethod]
        public async Task SelectSqlTest()
        {
            using Sqless sqless = new Sqless(SelectConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, SelectRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT TOP 2 [Name],[Price] FROM [Product]");
        }

        [TestMethod]
        public async Task SelectTest()
        {
            using Sqless sqless = new Sqless(SelectConfig);
            var result = await sqless.Select(SelectRequest);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task UnspecifiedFreeTableSelectTest()
        {
            var request = JsonConvert.DeserializeObject<SqlessSelectRequest>(JsonConvert.SerializeObject(SelectRequest));
            request.Table = Tables.AccessToken;

            using Sqless sqless = new Sqless(SelectConfig);
            try
            {
                var result = await sqless.Select(request);
            }
            catch (UnauthorizedAccessException)
            {
                Assert.IsTrue(true);
                return;
            }

            Assert.IsTrue(false);
        }
    }
}
