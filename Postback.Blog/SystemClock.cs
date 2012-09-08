using System;

namespace Postback.Blog
{
    public class SystemClock : ISystemClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}