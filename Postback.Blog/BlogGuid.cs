using System;

namespace Postback.Blog
{
    public class BlogGuid
    {
        public static Func<Guid> NewGuid = Guid.NewGuid;
    }
}