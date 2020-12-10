using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sqless.Demo.Common;
using Sqless.Request;

namespace Sqless.Test
{
    [TestClass]
    public class SqlessRequestTest
    {
        [TestMethod]
        public void LoadEditRequestFromObjectTest()
        {
            var obj = new
            {
                Test1 = "111",
                Test2 = "222"
            };
            var request = new SqlessEditRequest();

            request.LoadFromObject(obj);

            Assert.IsTrue(request.Fields.Count > 0);
            Assert.AreEqual(request.Fields[0].Field, nameof(obj.Test1)); ;
            Assert.AreEqual(request.Fields[0].Value, obj.Test1);
        }

        [TestMethod]
        public void LoadSelectRequestFromObjectTest()
        {
            var request = new SqlessSelectRequest();

            request.LoadFromType(typeof(User), Tables.User);

            Assert.IsTrue(request.Fields.Count > 0);
            Assert.AreEqual(request.Fields[0].Field, nameof(User.Uid));
            Assert.AreEqual(request.Fields[1].Field, nameof(User.Name));
        }
    }
}
