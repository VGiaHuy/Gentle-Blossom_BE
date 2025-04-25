using AutoMapper;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class PostService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PostDTO>> GetAllPost()
        {
            try
            {
                var posts = await _unitOfWork.Post.GetAllAsync();
                var allPost = new List<PostDTO>();

                foreach (var post in posts)
                {
                    var postDto = _mapper.Map<PostDTO>(post);

                    postDto.PosterName = post.Poster.FullName;
                    postDto.PosterAvatar = post.Poster.Avatar;
                    postDto.PosterType = post.Poster.UserType.TypeName;
                    postDto.CategoryName = post.Category.CategoryName;

                    postDto.AcademicTitle = post.Poster.UserType.UsertypeId == UserTypeName.Expert
                        ? post.Poster.Expert.AcademicTitle
                        : null!;

                    postDto.NumberOfComment = post.CommentPosts?.Count ?? 0;

                    allPost.Add(postDto);
                }

                return allPost;
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.ToString());
            }
        }
    }
}
