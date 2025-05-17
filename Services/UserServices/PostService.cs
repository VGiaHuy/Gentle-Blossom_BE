using AutoMapper;
using Ganss.Xss;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.GoogleService;

namespace GentleBlossom_BE.Services.UserServices
{
    public class PostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleDriveService _googleDriveService;
        private readonly ILogger<PostService> _logger;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context, GoogleDriveService googleDriveService, ILogger<PostService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _googleDriveService = googleDriveService;
            _logger = logger;
        }

        public async Task<List<PostDTO>> GetAllPost(int page = 1, int pageSize = 5)
        {
            try
            {
                var posts = await _unitOfWork.Post.GetAllAsync(page, pageSize);
                var allPost = new List<PostDTO>();

                foreach (var post in posts)
                {
                    var postDto = _mapper.Map<PostDTO>(post);

                    postDto.PosterName = post.Poster.FullName;
                    postDto.PosterAvatar = post.Poster.Avatar;
                    postDto.PosterType = post.Poster.UserType.TypeName;
                    postDto.CategoryName = post.Category.CategoryName;
                    postDto.AcademicTitle = post.Poster.UserType.UsertypeId == UserTypeName.Expert ? post.Poster.Expert.AcademicTitle : null!;
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

        public async Task<List<PostDTO>> GetPostsOfUserById(int id, int page = 1, int pageSize = 5)
        {
            try
            {
                var posts = await _unitOfWork.Post.GetPostsOfUserById(id, page, pageSize);
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

        public async Task CreatePostAsync(CreatePostDTO request)
        {
            var sanitizer = new HtmlSanitizer();

            if (!int.TryParse(request.UserId, out var posterId))
                throw new BadRequestException("Invalid UserId.");

            var post = new Post
            {
                PosterId = posterId,
                Content = sanitizer.Sanitize(request.Content), // vệ sinh HTML để loại bỏ mã nguy hiểm
                CategoryId = 1,
                NumberOfLike = 0
            };

            await _unitOfWork.Post.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            if (request.MediaFiles != null && request.MediaFiles.Count > 0)
            {
                var uploadTasks = request.MediaFiles.Select(async file =>
                {
                    try
                    {
                        var fileUrl = await _googleDriveService.UploadFileAsync(file, MediaType.Post);

                        var mediaType = string.IsNullOrEmpty(file.ContentType) ? throw new BadRequestException("Missing ContentType") :
                                        file.ContentType.StartsWith("image/") ? "Image" :
                                        file.ContentType.StartsWith("video/") ? "Video" :
                                        throw new BadRequestException($"Unsupported media type: {file.ContentType}");

                        return new PostMedium
                        {
                            PostId = post.PostId,
                            FileName = file.FileName,
                            MediaUrl = fileUrl,
                            MediaType = mediaType
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new InternalServerException($"Lỗi upload file: {file.FileName}: {ex.Message}");
                    }
                }).ToList();

                var mediaItems = await Task.WhenAll(uploadTasks);
                foreach (var media in mediaItems.Where(m => m != null))
                {
                    await _unitOfWork.PostMedia.AddAsync(media);
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
