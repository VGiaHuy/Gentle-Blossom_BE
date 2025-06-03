namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class CommentPostResponseDTO
    {
        public List<CommentPostDTOs> Comments { get; set; }
        public bool HasMore { get; set; }
    }
}
