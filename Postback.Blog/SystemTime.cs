using System;

namespace Postback.Blog
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}