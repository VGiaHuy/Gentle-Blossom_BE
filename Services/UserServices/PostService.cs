using AutoMapper;
using Ganss.Xss;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.AnalysisService;
using GentleBlossom_BE.Services.GoogleService;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Services.UserServices
{
    public class PostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleDriveService _googleDriveService;
        private readonly ILogger<PostService> _logger;
        private readonly PostAnalysisService _postAnalysisService;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context, GoogleDriveService googleDriveService, ILogger<PostService> logger, PostAnalysisService postAnalysisService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _googleDriveService = googleDriveService;
            _logger = logger;
            _postAnalysisService = postAnalysisService;
        }

        public async Task<List<PostDTO>> GetAllPost(int userId, int page = 1, int pageSize = 5)
        {
            try
            {
                var posts = await _unitOfWork.Post.GetAllAsync(userId, page, pageSize);

                return posts;
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
                return posts;
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

            // Khởi động phân tích bài viết bất đồng bộ
            await _postAnalysisService.AnalyzePostAsync(post);
        }

        public async Task CreateCommentAsync(CreateCommentDTOs request)
        {
            var sanitizer = new HtmlSanitizer();

            // Tạo đối tượng Comment
            var comment = new CommentPost
            {
                PostId = request.PostId,
                PosterId = request.PosterId,
                ParentCommentId = request.ParentCommentId,
                Content = sanitizer.Sanitize(request.Content)
            };

            // Xử lý file media nếu có
            if (request.MediaFile != null && request.MediaFile.Length > 0)
            {
                try
                {
                    // Kiểm tra định dạng file
                    var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "video/mp4", "video/webm" };
                    if (!allowedContentTypes.Contains(request.MediaFile.ContentType))
                    {
                        throw new BadRequestException($"Unsupported media type: {request.MediaFile.ContentType}. Only JPEG, PNG, GIF images or MP4, WebM videos are allowed.");
                    }

                    // Upload file lên Google Drive
                    var fileUrl = await _googleDriveService.UploadFileAsync(request.MediaFile, MediaType.Comment);

                    // Xác định loại media
                    var mediaType = string.IsNullOrEmpty(request.MediaFile.ContentType)
                        ? throw new BadRequestException("Missing ContentType")
                        : request.MediaFile.ContentType.StartsWith("image/") ? "Image"
                        : request.MediaFile.ContentType.StartsWith("video/") ? "Video"
                        : throw new BadRequestException($"Unsupported media type: {request.MediaFile.ContentType}");

                    comment.FileName = request.MediaFile.FileName;
                    comment.MediaType = mediaType;
                    comment.MediaUrl = fileUrl;
                }
                catch (Exception ex)
                {
                    throw new InternalServerException($"Lỗi upload file: {request.MediaFile.FileName}: {ex.Message}");
                }
            }

            // Thêm bình luận vào database
            await _unitOfWork.CommentPost.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<CommentPostResponseDTO> GetCommentsByPostIdAsync(int postId, int page, int pageSize)
        {
            try
            {
                var (comments, hasMore) = await _unitOfWork.CommentPost.GetCommentsByPostIdAsync(postId, page, pageSize);
                var commentDtos = new List<CommentPostDTOs>();

                foreach (var comment in comments)
                {
                    var commentDto = _mapper.Map<CommentPostDTOs>(comment);
                    commentDto.FullName = comment.Poster.FullName;
                    commentDto.PosterAvatarUrl = comment.Poster.AvatarUrl;
                    commentDto.PosterAvatarType = comment.Poster.AvatarType;
                    commentDto.PosterAvatarFileName = comment.Poster.AvatarFileName;
                    commentDto.PosterType = comment.Poster.UserType.TypeName;

                    commentDtos.Add(commentDto);
                }

                return new CommentPostResponseDTO
                {
                    Comments = commentDtos,
                    HasMore = hasMore
                };
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.ToString());
            }
        }

        public async Task<bool> ToggleLikePost(int postId, int userId)
        {
            try
            {
                bool checkLikePost = await _unitOfWork.PostLike.CheckLikePost(postId, userId);

                // nếu đã thích thì handle = -1 (bỏ thích), nếu chưa thích thì handle = 1 (thích)
                int handle = checkLikePost ? -1 : 1;

                if (checkLikePost)
                {
                    await _unitOfWork.PostLike.UnLikePost(postId, userId);
                }
                else
                {
                    await _unitOfWork.PostLike.LikePost(postId, userId);
                }

                bool likePost = await _unitOfWork.Post.ToggleLikePost(postId, handle);

                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new InternalServerException($"Error toggling like for post {postId} by user {userId}: {ex.Message}");
            }
        }
    }
}
