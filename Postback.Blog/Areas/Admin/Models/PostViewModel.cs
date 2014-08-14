namespace Postback.Blog.Areas.Admin.Models
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Created { get; set; }
        public string PublishFrom { get; set; }
        public string Id { get; set; }
        public string Uri { get; set; }
        public bool Active { get; set; }
        public bool IsPublished { get; set; }
    }
}