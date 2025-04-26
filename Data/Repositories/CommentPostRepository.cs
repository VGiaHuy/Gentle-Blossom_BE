using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class CommentPostRepository : GenericRepository<CommentPost>, ICommentPostRepository
    {
        public CommentPostRepository(GentleBlossomContext context) : base(context)
        {
        }
    }
}
