using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<PostDTO>> GetAllAsync(int userId, int page = 1, int pageSize = 5)
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(post => new PostDTO
                {
                    PostId = post.PostId,
                    Content = post.Content,
                    CreatedDate = post.CreatedDate,
                    NumberOfLike = post.NumberOfLike,
                    Liked = post.PostLikes.Any(pl => pl.UserId == userId),
                    PosterName = post.Poster.FullName,
                    PosterAvatar = new AvatarMediaDTO
                    {
                        AvatarUrl = post.Poster.AvatarUrl ?? string.Empty,
                        AvatarType = post.Poster.AvatarType ?? string.Empty,
                        AvatarFileName = post.Poster.AvatarFileName ?? string.Empty,
                    },
                    PosterType = post.Poster.UserType.TypeName,
                    AcademicTitle = post.Poster.UserType.UsertypeId == UserTypeName.Expert
                                        ? post.Poster.Expert.AcademicTitle
                                        : null!,
                    CategoryName = post.Category.CategoryName,
                    NumberOfComment = post.CommentPosts.Count(),
                    MediaList = post.PostMedia.Select(m => new PostMediaDTO
                    {
                        MediaUrl = m.MediaUrl,
                        MediaType = m.MediaType,
                        FileName = m.FileName
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostDTO>> GetPostsOfUserById(int id, int page = 1, int pageSize = 5)
        {
            return await _context.Posts
                .Where(u => u.PosterId == id)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(post => new PostDTO
                {
                    PostId = post.PostId,
                    Content = post.Content,
                    CreatedDate = post.CreatedDate,
                    NumberOfLike = post.NumberOfLike,
                    Liked = post.PostLikes.Any(pl => pl.UserId == id),
                    PosterName = post.Poster.FullName,
                    PosterAvatar = new AvatarMediaDTO
                    {
                        AvatarUrl = post.Poster.AvatarUrl ?? string.Empty,
                        AvatarType = post.Poster.AvatarType ?? string.Empty,
                        AvatarFileName = post.Poster.AvatarFileName ?? string.Empty,
                    },
                    PosterType = post.Poster.UserType.TypeName,
                    AcademicTitle = post.Poster.UserType.UsertypeId == UserTypeName.Expert
                                        ? post.Poster.Expert.AcademicTitle
                                        : null!,
                    CategoryName = post.Category.CategoryName,
                    NumberOfComment = post.CommentPosts.Count(),
                    MediaList = post.PostMedia.Select(m => new PostMediaDTO
                    {
                        MediaUrl = m.MediaUrl,
                        MediaType = m.MediaType,
                        FileName = m.FileName
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<bool> ToggleLikePost(int posId, int handle)
        {
            int affectedRows = _context.Posts
                .Where(p => p.PostId == posId)
                .ExecuteUpdate(p => p.SetProperty(x => x.NumberOfLike, x => x.NumberOfLike + handle));

            if (affectedRows > 0)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
