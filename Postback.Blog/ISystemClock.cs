using System;

namespace Postback.Blog
{
    public interface ISystemClock
    {
        DateTime Now();
    }
}