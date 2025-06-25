using AutoMapper;
using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();

            CreateMap<LoginUser, RegisterViewModel>().ReverseMap();
            CreateMap<UserProfile, RegisterViewModel>().ReverseMap();
            CreateMap<UserProfile, AdminProfileDTO>()
                .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.Administrator.AdminId))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Administrator.Role.RoleName))
                .ReverseMap();

            CreateMap<Post, PostDTO>().ForMember(dest => dest.MediaList, opt => opt.MapFrom(src => src.PostMedia));
            CreateMap<PostMedium, PostMediaDTO>();
            CreateMap<CommentPost, CommentPostDTOs>().ReverseMap();

            CreateMap<UserProfile, PostDTO>().ReverseMap();

            CreateMap<HealthJourney, PsychologyDiaryDTO>().ReverseMap();
            CreateMap<HealthJourney, PeriodicHealthDTO>().ReverseMap();

            CreateMap<PeriodicHealth, PeriodicHealthDTO>()
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Journey.Treatment.TreatmentName))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Journey.DueDate))
                .ReverseMap();

            CreateMap<PsychologyDiary, PsychologyDiaryDTO>()
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Journey.Treatment.TreatmentName))
                .ReverseMap();

            CreateMap<Notification, NotificationDTO>()
                //.ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src.User))
                .ReverseMap();

            CreateMap<ChatRoom, ChatRoomDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();

            CreateMap<HealthJourney, HealthJourneyDTO>()
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment.TreatmentName))
                .ReverseMap();

            CreateMap<MonitoringForm, MonitoringFormDTO>()
                .ForMember(dest => dest.ExpertName, opt => opt.MapFrom(src => src.Expert.User.FullName))
                .ForMember(dest => dest.ExpertAcademicTitle, opt => opt.MapFrom(src => src.Expert.AcademicTitle))
                .ReverseMap();
        }
    }
}
