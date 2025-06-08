namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }

        public int ChatRoomId { get; set; }

        public int SenderId { get; set; }

        public string? Content { get; set; }

        public bool? HasAttachment { get; set; }

        public DateTime? SentAt { get; set; }

        public bool? IsDeleted { get; set; }

        public bool IsOutgoing { get; set; }

        public string? AvatarUrl { get; set; }

    }
}
