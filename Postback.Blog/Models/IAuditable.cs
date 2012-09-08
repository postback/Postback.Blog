using System;

namespace Postback.Blog.Models
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime Created { get; set; }
    }
}