using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class ExpertRepository : GenericRepository<Expert>, IExpertRepository
    {
        public ExpertRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<ExpertProfileDTO>> GetAllExperts()
        {
            return await _context.Experts
                .Include(user => user.User)
                .Select(x => new ExpertProfileDTO
                {
                    AcademicTitle = x.AcademicTitle,
                    AvatarUrl = x.User.AvatarUrl,
                    Description = x.Description,
                    ExpertId = x.ExpertId,
                    FullName = x.User.FullName,
                    Gender = x.User.Gender,
                    Organization = x.Organization,
                    Position = x.Position,
                    Specialization = x.Specialization
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(List<ExpertProfileDTO>, int)> GetAllExpertsAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int skip = (page - 1) * pageSize;

            var query = _context.Experts
                .AsNoTracking()
                .Include(user => user.User)
                .Select(x => new ExpertProfileDTO
                {
                    ExpertId = x.ExpertId,
                    FullName = x.User.FullName,
                    Gender = x.User.Gender,
                    Organization = x.Organization,
                    Position = x.Position,
                    Specialization = x.Specialization,
                    AcademicTitle = x.AcademicTitle,
                    Description = x.Description,
                    AvatarUrl = x.User.AvatarUrl,
                })
                .OrderBy(k => k.ExpertId);

            int totalCount = await query.CountAsync();
            var experts = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return (experts, totalCount);
        }

        public async Task<Expert> GetExpertByUserIdAsync(int expertId)
        {
            return await _context.Experts.FirstAsync(e => e.UserId == expertId);
        }

        public async Task<ExpertProfileDTO> GetExpertsProfile(int expertId)
        {
            return await _context.Experts
                .Where(expert => expert.ExpertId == expertId)
                .Include(user => user.User)
                .Select(x => new ExpertProfileDTO
                {
                    AcademicTitle = x.AcademicTitle,
                    AvatarUrl = x.User.AvatarUrl,
                    Description = x.Description,
                    ExpertId = x.ExpertId,
                    FullName = x.User.FullName,
                    Gender = x.User.Gender,
                    Organization = x.Organization,
                    Position = x.Position,
                    Specialization = x.Specialization
                })
                .AsNoTracking()
                .FirstAsync();
        }
    }
}
