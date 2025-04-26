using System.ComponentModel.DataAnnotations;

namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [MinLength(5, ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự.")]
        [MaxLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 100 ký tự.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        public string FullName { get; set; }

        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và phải đúng định dạng.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }

        public string? Avatar { get; set; }

        public bool Gender { get; set; }
    }
}
