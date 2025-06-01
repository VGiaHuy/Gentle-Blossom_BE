using AutoMapper;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();

            CreateMap<LoginUser, RegisterRequestDTO>().ReverseMap();
            CreateMap<UserProfile, RegisterRequestDTO>().ReverseMap();

            CreateMap<Post, PostDTO>().ForMember(dest => dest.MediaList, opt => opt.MapFrom(src => src.PostMedia));
            CreateMap<PostMedium, PostMediaDTO>();
            CreateMap<CommentPost, CommentPostDTOs>().ReverseMap();

            CreateMap<UserProfile, PostDTO>().ReverseMap();

            CreateMap<HealthJourney, PsychologyDiaryDTO>().ReverseMap();
            CreateMap<HealthJourney, PeriodicHealthDTO>().ReverseMap();
        }
    }
}
