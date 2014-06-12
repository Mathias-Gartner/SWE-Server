using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Fake;
using ErpPlugin.Data;

namespace ErpUnitTests
{
    [TestClass]
    public class UserTests : ErpBaseTest
    {
        [TestMethod]
        public void PasswordHashTest()
        {
            var hash = User.CreatePasswordHash("123456", "1234");
            Assert.AreEqual("EEC39BDD6FDBE97DDD8C6FE18141043038DA1DE9", hash);
        }
    }
}
