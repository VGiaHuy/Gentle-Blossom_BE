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

        public async Task<List<Post>> GetAllAsync(int page = 1, int pageSize = 5)
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.UserType)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.Expert)
                .Include(p => p.Category)
                .Include(a => a.PostMedia)
                .AsNoTracking() // Tăng hiệu suất
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsOfUserById(int id, int page = 1, int pageSize = 5)
        {
            return await _context.Posts
                .Where(u => u.PosterId == id)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.UserType)
                .Include(p => p.Poster)
                    .ThenInclude(u => u.Expert)
                .Include(p => p.Category)
                .Include(a => a.PostMedia)
                .AsNoTracking() // Tăng hiệu suất
                .ToListAsync();
        }
    }
}
