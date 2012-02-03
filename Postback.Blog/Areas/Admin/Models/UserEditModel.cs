using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using Norm;

namespace Postback.Blog.Areas.Admin.Models
{
    public class UserEditModel
    {
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Email]
        [Remote("isunique", "user", "api", AdditionalFields = "Id")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EqualTo("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}