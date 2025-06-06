namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class NotificationResponseDTO
    {
        public List<NotificationDTO> Notifications { get; set; } = new List<NotificationDTO>();
        public int TotalCount { get; set; }
    }
}
