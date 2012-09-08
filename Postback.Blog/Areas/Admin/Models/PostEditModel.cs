using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Postback.Blog.Areas.Admin.Models
{
    public class PostEditModel
    {
        public PostEditModel()
        {
            PublishFrom = DateTime.Now;
        }

        public string Id { get; set; }

        [Required]
        [Remote("isunique", "post", "api", AdditionalFields = "Id")]
        public string Title { get; set; }

        [Required]
        public string Tags { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public string Body { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishFrom { get; set; }
    }
}