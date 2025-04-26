using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts
                .Include(p => p.Poster)
                    .ThenInclude(u => u.UserType)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.Expert)
                .Include(p => p.Category)
                .Include(p => p.CommentPosts)
                                .Include(a => a.PostImages)

                .ToListAsync();
        }
    }
}
