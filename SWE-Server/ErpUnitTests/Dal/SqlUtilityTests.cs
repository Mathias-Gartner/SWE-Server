using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data.Database;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class SqlUtilityTests
    {
        [TestMethod]
        public void ExtractParameters()
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", 2);
            arguments.Add("username", "testuser");
            var parameters = SqlUtility.ExtractParameters(arguments).ToArray();
            Assert.AreEqual("@id", parameters[0].ParameterName);
            Assert.AreEqual(2, parameters[0].Value);
            Assert.AreEqual("@username", parameters[1].ParameterName);
            Assert.AreEqual("testuser", parameters[1].Value);
        }

        [TestMethod]
        public void AppendWhereClause()
        {
            var sb = new StringBuilder("SELECT * FROM users");
            var arguments = new Dictionary<string,object>();
            arguments.Add("id", 2);
            arguments.Add("username", "testuser");
            SqlUtility.AppendWhereClause(sb, arguments);
            Assert.AreEqual("SELECT * FROM users WHERE id=@id and username=@username", sb.ToString());
        }

        [TestMethod]
        public void PrepareSelect()
        {
            var sb = SqlUtility.PrepareSelect(new UserDefinition());
            Assert.AreEqual("SELECT users.id, users.username, users.password, users.passwordSalt FROM users", sb.ToString());
        }

        [TestMethod]
        public void PrepareInsert()
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", 2);
            arguments.Add("password", "123456");
            var sb = SqlUtility.PrepareInsert(new UserDefinition(), arguments);
            Assert.AreEqual("INSERT INTO users(id, password) OUTPUT INSERTED.id VALUES (@id, @password)", sb.ToString());
        }

        [TestMethod]
        public void PrepareUpdate()
        {
            var arguments = new Dictionary<string, object>();
            arguments.Add("id", 2);
            arguments.Add("password", "123456");
            var sb = SqlUtility.PrepareUpdate(new UserDefinition(), arguments);
            Assert.AreEqual("UPDATE users SET id=@id, password=@password OUTPUT INSERTED.id", sb.ToString());
        }

        [TestMethod]
        public void PrepareDelete()
        {
            var sb = SqlUtility.PrepareDelete(new UserDefinition());
            Assert.AreEqual("DELETE FROM users", sb.ToString());
        }
    }
}
