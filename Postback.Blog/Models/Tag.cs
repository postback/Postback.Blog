namespace Postback.Blog.Models
{
    public class Tag : Entity
    {
        public Tag():base()
        {
        }

        public Tag(string name)
            : this()
        {
            Uri = name.ToUri();
            Name = name;
        }

        public string Name { get; set; }
        public string Uri { get; set; }
    }
}