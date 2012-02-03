using System.Web.Mvc;
using Postback.Blog.App.Services;

namespace Postback.Blog.Models
{
    public class User : Entity
    {
        public User()
            : base()
        {
        }

        public User(string password):this()
        {
            if (!string.IsNullOrEmpty(password))
            {
                var cryptographer = DependencyResolver.Current.GetService<ICryptographer>();
                PasswordSalt = cryptographer.CreateSalt();
                PasswordHashed = cryptographer.GetPasswordHash(password, PasswordSalt);
            }
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHashed { get; set; }
        public string PasswordSalt { get; set; }
        public bool Active { get; set; }
    }
}