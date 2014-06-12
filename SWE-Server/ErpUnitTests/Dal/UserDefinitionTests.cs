using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class UserDefinitionTests
    {
        [TestMethod]
        public void CreateArguments()
        {
            var user = new User();
            user.PasswordHash = "EEC39BDD6FDBE97DDD8C6FE18141043038DA1DE9";
            user.PasswordSalt = "1234";
            user.Username = "test";

            var dal = new UserDefinition();
            var a = dal.CreateArguments(user);

            Assert.AreEqual("EEC39BDD6FDBE97DDD8C6FE18141043038DA1DE9", a["password"]);
            Assert.AreEqual("1234", a["passwordSalt"]);
            Assert.AreEqual("test", a["username"]);
        }
    }
}
