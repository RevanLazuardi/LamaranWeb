using AutoMapper;
using LamaranWeb.Models;
namespace LamaranWeb.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicantDTO, Applicant>()
                
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now)); 

            CreateMap<Applicant, ApplicantDTO>()
                .ForMember(dest => dest.CV, opt => opt.Ignore());
        }
    }
}


