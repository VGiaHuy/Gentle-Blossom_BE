using GentleBlossom_BE.Data.DTOs.UserDTOs;

namespace GentleBlossom_BE.Data.DTOs
{
    public class UserProfileViewModel
    {
        public UserProfileDTO UserProfile { get; set; }
        public List<PsychologyDiaryDTO>? PsychologyDiaries { get; set; }
        public List<PeriodicHealthDTO>? PeriodicHealths { get; set; }
    }
}
