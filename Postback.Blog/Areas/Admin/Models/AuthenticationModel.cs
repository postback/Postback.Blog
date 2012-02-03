using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace Postback.Blog.Areas.Admin.Models
{
    public class AuthenticationModel
    {
        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}