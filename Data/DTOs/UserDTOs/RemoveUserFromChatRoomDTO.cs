namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class RemoveUserFromChatRoomDTO
    {
        public int ChatRoomId { get; set; }
        public int ParticipantId { get; set; }
    }
}
