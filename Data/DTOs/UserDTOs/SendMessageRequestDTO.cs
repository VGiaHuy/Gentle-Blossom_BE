namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class SendMessageRequestDTO
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
