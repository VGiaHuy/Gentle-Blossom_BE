using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PostMediaRepository : GenericRepository<PostMedium>, IPostMediaRepository
    {
        public PostMediaRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task DeleteRangeByMediaUrlAsync(string url, int postId)
        {
            var postMedia = await _context.PostMedia
                .Where(pm => pm.PostId == postId && pm.MediaUrl == url)
                .FirstAsync();

            if (postMedia != null)
            {
                _context.PostMedia.RemoveRange(postMedia);
            }
        }

        public async Task DeleteRangeByPostIdAsync(int postId)
        {
            var postMedia = await _context.PostMedia
                .Where(pm => pm.PostId == postId)
                .ToListAsync();

            if (postMedia.Any())
            {
                _context.PostMedia.RemoveRange(postMedia);
            }
        }
    }
}
