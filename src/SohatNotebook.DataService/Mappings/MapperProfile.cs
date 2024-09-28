using AutoMapper;
using SohatNotebook.Entities.Dtos.Incomming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.Dtos.Incomming.Profile;

namespace SohatNotebook.Configurations.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Add mappings here
            CreateMap<UserDTO, User>()
                .ForMember(
                    dest => dest.FirstName,
                    from => from.MapFrom(x => $"{x.FirstName}"))
                .ForMember(dest => dest.LastName,
                    from => from.MapFrom(x => $"{x.LastName}"))
                .ForMember(dest => dest.Email,
                    from => from.MapFrom(x => $"{x.Email}"))
                .ForMember(dest => dest.Phone,
                    from => from.MapFrom(x => $"{x.Phone}"))
                .ForMember(dest => dest.Country,
                    from => from.MapFrom(x => $"{x.Country}"))
                .ForMember(dest => dest.Datebirth,
                    from => from.MapFrom(x => $"{Convert.ToDateTime(x.Datebirth)}"))
                .ForMember(dest => dest.Status,
                    from => from.MapFrom(x => 1))
                .ForMember(dest => dest.Address,
                    from => from.MapFrom(x => ""))
                .ForMember(dest => dest.MobileNumber,
                    from => from.MapFrom(x => ""))
                .ForMember(dest => dest.Sex,
                    from => from.MapFrom(x => ""));

            CreateMap<ProfileDTO, User>()
                .ForMember(dest => dest.Country,
                    from => from.MapFrom(x => $"{x.Country}"))
                .ForMember(dest => dest.MobileNumber,
                    from => from.MapFrom(x => $"{x.MobileNumber}"))
                .ForMember(dest => dest.Address,
                    from => from.MapFrom(x => $"{x.Address}"))
                .ForMember(dest => dest.Sex,
                    from => from.MapFrom(x => $"{x.Sex}"));
        }
    }
}