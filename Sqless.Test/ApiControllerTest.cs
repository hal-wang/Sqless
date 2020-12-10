using Sqless.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sqless.Test
{
    [TestClass]
    public class ApiControllerTest
    {
        [TestMethod]
        public void SqlessApiTypeTest()
        {
            var types = new List<string>();
            foreach (var enumValue in Enum.GetValues(typeof(SqlessApiType)))
            {
                types.Add(Enum.GetName(typeof(SqlessApiType), enumValue));
            }

            var controllerActions = typeof(SqlessBaseController).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(item => item.CustomAttributes.Where(ca => ca.AttributeType == typeof(SqlessApiActionAttribute)).Count() > 0)
                .Select(item => item.Name)
                .ToList();

            var clientActions = typeof(SqlessClient).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(item => item.CustomAttributes.Where(ca => ca.AttributeType == typeof(SqlessApiActionAttribute)).Count() > 0)
                .Select(item => item.Name)
                .ToList();

            Assert.IsTrue(controllerActions.Where(ca => types.Where(t => t == ca).Count() == 0).Count() == 0);
            Assert.IsTrue(clientActions.Where(ca => types.Where(t => t == ca).Count() == 0).Count() == 0);

            Assert.IsTrue(types.Where(t => controllerActions.Where(ca => ca == t).Count() == 0).Count() == 0);
            Assert.IsTrue(types.Where(t => clientActions.Where(ca => ca == t).Count() == 0).Count() == 0);
        }
    }
}
