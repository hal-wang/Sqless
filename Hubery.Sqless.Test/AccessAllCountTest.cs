using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hubery.Sqless.Demo.Common;
using Hubery.Sqless.Request;
using Hubery.Sqless.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hubery.Sqless.Test
{
    [TestClass]
    public class AccessAllCountTest
    {
        private SqlessCountRequest SqlessCountRequest
        {
            get
            {
                var result = new SqlessCountRequest()
                {
                    Table = Tables.User
                };
                return result;
            }
        }

        [TestMethod]
        public async Task CountSqlTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessCountSqlBuilder producer = new SqlessCountSqlBuilder(sqless, SqlessCountRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT COUNT(*) FROM [User]");
        }

        [TestMethod]
        public async Task CountTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Count(SqlessCountRequest);

            Assert.IsTrue(selectResult > 0);
        }


        private SqlessCountRequest SqlessCountDistinctRequest
        {
            get
            {
                var result = new SqlessCountRequest()
                {
                    Table = Tables.User,
                    Distinct = true,
                    Field = "Uid"
                };
                return result;
            }
        }

        [TestMethod]
        public async Task CountDistinctSqlTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessCountSqlBuilder producer = new SqlessCountSqlBuilder(sqless, SqlessCountDistinctRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT COUNT(DISTINCT [Uid]) FROM [User]");
        }

        [TestMethod]
        public async Task CountDistinctTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Count(SqlessCountDistinctRequest);

            Assert.IsTrue(selectResult > 0);
        }
    }
}
