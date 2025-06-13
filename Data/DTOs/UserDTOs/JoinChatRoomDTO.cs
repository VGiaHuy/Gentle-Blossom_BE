namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class JoinChatRoomDTO
    {
        public int UserId { get; set; }

        public string ChatCode { get; set; } = string.Empty;
    }
}
