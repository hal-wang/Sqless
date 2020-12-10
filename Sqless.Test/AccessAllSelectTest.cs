using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sqless.Demo.Common;
using Sqless.Query;
using Sqless.Request;
using Sqless.SqlBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqless.Test
{
    [TestClass]
    public class AccessAllSelectTest
    {
        #region SelectFieldTest
        private SqlessSelectRequest SelectFieldTestRequest
        {
            get
            {
                var result = new SqlessSelectRequest()
                {
                    Table = Tables.User,
                    PageSize = 20,
                    Fields = new List<SqlessField>()
                    {
                        new SqlessField()
                        {
                            Field = "Uid",
                        },
                        new SqlessField()
                        {
                            Field="Name"
                        }
                    }
                };
                return result;
            }
        }

        [TestMethod]
        public async Task SelectFieldSqlTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, SelectFieldTestRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT TOP 20 [Uid],[Name] FROM [User]");
        }

        [TestMethod]
        public async Task SelectFieldTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Select(SelectFieldTestRequest);

            Assert.IsTrue(selectResult.Count > 0 && (selectResult[0].Uid) == "1");
        }
        #endregion


        #region SelectPageTest
        private SqlessSelectRequest SelectPageTestRequest
        {
            get
            {
                return new SqlessSelectRequest()
                {
                    Table = Tables.User,
                    PageSize = 2,
                    PageIndex = 1,
                    Orders = new List<SqlessOrder>()
                    {
                        new SqlessOrder() { Field="Uid" }
                    },
                    Fields = new List<SqlessField>()
                    {
                        new SqlessField() {  Field="Uid"  },
                        new SqlessField() {  Field="Name"  }
                    }
                };
            }
        }

        [TestMethod]
        public async Task SelectPageSqlTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, SelectPageTestRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT [Uid],[Name] FROM [User] ORDER BY [Uid] Asc OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY");
        }

        [TestMethod]
        public async Task SelectPageTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Select(SelectPageTestRequest);

            Assert.IsTrue(selectResult.Count > 0 && (selectResult[0].Uid) == "3");
        }
        #endregion

        #region SelectNullTest
        private SqlessSelectRequest SelectNullTestRequest
        {
            get
            {
                return new SqlessSelectRequest()
                {
                    Table = Tables.User,
                    Queries = new List<SqlessQuery>()
                    {
                        new SqlessQuery()
                        {
                            Field="Phone",
                            Type=SqlessQueryType.Null
                        }
                    },
                    Fields = new List<SqlessField>()
                    {
                        new SqlessField() {  Field="Name"  }
                    }
                };
            }
        }

        [TestMethod]
        public async Task SelectNullSqlTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, SelectNullTestRequest);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT [Name] FROM [User] WHERE [Phone] IS NULL");
        }

        [TestMethod]
        public async Task SelectNullTest()
        {
            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Select(SelectNullTestRequest);
            Assert.IsTrue(selectResult.Count > 0);
        }
        #endregion

        private SqlessSelectRequest JoinRequest
        {
            get
            {
                return new SqlessSelectRequest()
                {
                    Table = Tables.User,
                    Queries = new List<SqlessQuery>()
                    {
                        new SqlessQuery()
                        {
                            Field="Phone",
                            Table="User",
                            Type=SqlessQueryType.NotNull
                        }
                    },
                    Fields = new List<SqlessField>()
                    {
                        new SqlessField() {  Table=Tables.User, Field="Name"  },
                        new SqlessField() {  Table=Tables.User, Field="Uid"  },
                        new SqlessField() {  Table=Tables.Order, Field="Time"  }
                    },
                    Joins = new List<SqlessJoin>()
                    {
                        new SqlessJoin()
                        {
                            LeftTable=Tables.User,
                            LeftField="Uid",
                            RightTable=Tables.Order,
                            RightField="Uid"
                        }
                    }
                };
            }
        }


        #region LeftJoinTest
        [TestMethod]
        public async Task LeftJoinSqlTest()
        {
            var request = JoinRequest;
            request.Joins[0].SqlessJoinType = SqlessJoinType.LeftJoin;

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, request);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT [User].[Name],[User].[Uid],[Order].[Time] FROM [User] LEFT JOIN [Order] ON [User].[Uid] = [Order].[Uid] WHERE [User].[Phone] IS NOT NULL");
        }

        [TestMethod]
        public async Task LeftJoinTest()
        {
            var request = JoinRequest;
            request.Joins[0].SqlessJoinType = SqlessJoinType.LeftJoin;

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Select(request);
            Assert.IsTrue(selectResult.Count > 4);
        }
        #endregion



        #region RightJoinTest

        [TestMethod]
        public async Task RightJoinSqlTest()
        {
            var request = JoinRequest;
            request.Joins[0].SqlessJoinType = SqlessJoinType.RightJoin;

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            SqlessSelectSqlBuilder producer = new SqlessSelectSqlBuilder(sqless, request);
            var sqlStr = await producer.GetSqlStrTest();
            Assert.AreEqual(sqlStr.Replace("  ", " ").Trim(), "SELECT [User].[Name],[User].[Uid],[Order].[Time] FROM [User] RIGHT JOIN [Order] ON [User].[Uid] = [Order].[Uid] WHERE [User].[Phone] IS NOT NULL");
        }

        [TestMethod]
        public async Task RightJoinTest()
        {
            var request = JoinRequest;
            request.Joins[0].SqlessJoinType = SqlessJoinType.RightJoin;

            using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
            var selectResult = await sqless.Select(request);
            Assert.IsTrue(selectResult.Count > 0);
        }
        #endregion
    }
}