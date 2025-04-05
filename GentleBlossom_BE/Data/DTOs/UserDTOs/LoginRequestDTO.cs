using System.ComponentModel.DataAnnotations;

namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [MinLength(5, ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự.")]
        [MaxLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 100 ký tự.")]
        public string Password { get; set; }
    }
}