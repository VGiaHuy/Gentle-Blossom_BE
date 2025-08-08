namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class ForgotPasswordResponse
    {
        public string OtpToken { get; set; }
        public string Email { get; set; }
    }
}
