using AutoMapper;
using Postback.Blog.App.Mapping;

namespace Postback.Blog.App.Bootstrap
{
    public class AutoMapperBootstrap : IStartUpTask
    {
        public void Configure()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}