using AutoMapper;
using Hourglass.Api.Auth;
using Hourglass.Api.Service;
using Hourglass.Api.User;
using Hourglass.Site.Entities;
using System.Security;

namespace Hourglass.Site {
	public class MappingProfile : Profile {
		public MappingProfile() {

			CreateMap<RegistrationRequest, User>()
				.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email))
				.ForMember(u => u.SecurityStamp, opts => opts.MapFrom(x => Guid.NewGuid().ToString()));
		}
	}
}
