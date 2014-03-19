using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data.Database;
using System.Text;
using System.Collections.Generic;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class DalTests
    {
        [TestMethod]
        public void AppendWhereClause()
        {
            var sb = new StringBuilder("SELECT * FROM Users");
            var arguments = new Dictionary<string,object>();
            arguments.Add("id", 2);
            arguments.Add("username", "testuser");
            ErpPlugin.Data.Database.Dal.AppendWhereClause(sb, arguments);
            Assert.AreEqual("SELECT * FROM Users WHERE id=@id and username=@username", sb.ToString());
        }

        [TestMethod]
        public void PrepareSelect()
        {
            var sb = ErpPlugin.Data.Database.Dal.PrepareSelect(new UserDal());
            Assert.AreEqual("SELECT id, username, password, passwordSalt FROM users", sb.ToString());
        }

        [TestMethod]
        public void PrepareUpdate()
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", 2);
            arguments.Add("password", "123456");
            var sb = ErpPlugin.Data.Database.Dal.PrepareUpdate(new UserDal(), arguments);
            Assert.AreEqual("UPDATE users SET id=@id, password=@password", sb.ToString());
        }

        [TestMethod]
        public void PrepareDelete()
        {
            var sb = ErpPlugin.Data.Database.Dal.PrepareDelete(new UserDal());
            Assert.AreEqual("DELETE FROM users", sb.ToString());
        }
    }
}
