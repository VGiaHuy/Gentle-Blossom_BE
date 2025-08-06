namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class VerifyOtpRequest
    {
        public string OtpToken { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
