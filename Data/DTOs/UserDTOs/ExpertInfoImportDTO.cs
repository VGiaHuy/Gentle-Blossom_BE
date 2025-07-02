namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class ExpertInfoImportDTO
    {
        public int ExpertId { get; set; }

        public string FullName { get; set; } = null!;

        public bool Gender { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public string? AvatarUrl { get; set; }

        public string AcademicTitle { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string Specialization { get; set; } = null!;

        public string Organization { get; set; } = null!;

        public string? Description { get; set; }
    }
}
