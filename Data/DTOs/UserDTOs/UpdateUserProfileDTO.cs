﻿namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class UpdateUserProfileDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IFormFile? Avatar { get; set; }

        public bool Gender { get; set; }

        public byte UserTypeId { get; set; }
    }
}
