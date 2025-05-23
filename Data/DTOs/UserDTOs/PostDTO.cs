﻿namespace GentleBlossom_BE.Data.DTOs.UserDTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }

        public string PosterName { get; set; }

        public string? PosterAvatar { get; set; }

        public string PosterType { get; set; }

        public string AcademicTitle { get; set; }

        public string CategoryName { get; set; }

        public string Content { get; set; } = null!;

        public DateOnly? CreatedDate { get; set; }

        public int NumberOfLike { get; set; }

        public int NumberOfComment { get; set; }
        public List<PostMediaDTO> MediaList { get; set; } = new();
    }

    public class PostMediaDTO
    {
        public string MediaUrl { get; set; } = null!;
        public string MediaType { get; set; } = null!; // "image", "video", ...
        public string? FileName { get; set; }
        public string? MediaData { get; set; }
    }
}
