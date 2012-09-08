using System;

namespace Postback.Blog.Models
{
    public interface IAuditable
    {
        User Author { get; set; }
        DateTime Created { get; set; }
    }
}