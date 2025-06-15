namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class JwtResponseDTO
    {
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
