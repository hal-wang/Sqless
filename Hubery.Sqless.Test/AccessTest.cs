using Hubery.Sqless.Demo.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Hubery.Sqless.Test
{
    [TestClass]
    public class AccessTest
    {
        [TestMethod]
        public async Task GetUidByPasswordTest()
        {
            var uid = await Global.PasswordAccessConfig.GetUid("AccessTestUid", "123456");
            Assert.IsTrue(!string.IsNullOrEmpty(uid));
        }

        [TestMethod]
        public async Task GetUidByTokenTest()
        {
            var uid = await Global.TokenAccessConfig.GetUid("AccessTestToken", "Wpf");
            Assert.IsTrue(!string.IsNullOrEmpty(uid));
        }
    }
}
