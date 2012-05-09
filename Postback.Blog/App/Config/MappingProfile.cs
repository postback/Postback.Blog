using Postback.Blog.Models;

namespace Postback.Blog.App.Config
{
    public class MappingProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<InitialUserModel, User>()
                .ForMember(x => x.Active, o => o.UseValue(true))
                .ForMember(u => u.PasswordHashed, o => o.Ignore())
                .ForMember(u => u.PasswordSalt, o => o.Ignore())
                .ForMember(u => u.Id, o => o.Ignore());
        }
    }
}