using ErpPlugin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ErpPlugin
{
    public class User : BusinessObject
    {
        public User()
        {
        }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public void SetPassword(string password)
        {
            if (String.IsNullOrEmpty(PasswordSalt))
                PasswordSalt = CreateSalt();

            PasswordHash = CreatePasswordHash(password, PasswordSalt);
        }

        public ICollection<User> Search()
        {
            if (State != BusinessObjectState.SearchObject)
                throw new InvalidOperationException("Only SearchObject can be used for searching");

            return CurrentDalFactory.Instance.CreateDal().Search(this);
        }

        public static User CreateSearchObject()
        {
            return CreateSearchObject<User>();
        }

        public static string CreateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[64];
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        public static string CreatePasswordHash(string password, string salt)
        {
            string salted = String.Concat(password, salt);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(salted, "sha1");
            return hash;
        }
    }
}
