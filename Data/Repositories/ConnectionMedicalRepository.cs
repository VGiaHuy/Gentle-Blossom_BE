using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class ConnectionMedicalRepository : GenericRepository<ConnectionMedical>, IConnectionMedicalRepository
    {
        public ConnectionMedicalRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<bool> DeleteByPostId(int postId)
        {
            var connectionMedical = await _context.ConnectionMedicals.FirstOrDefaultAsync(pa => pa.PostId == postId);
            if (connectionMedical != null)
            {
                _context.ConnectionMedicals.Remove(connectionMedical);
                return true;
            }
            return false;
        }
    }
}
