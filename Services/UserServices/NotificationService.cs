using Microsoft.AspNetCore.Mvc;
using GentleBlossom_BE.Data.DTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using AutoMapper;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Services.AnalysisService;
using GentleBlossom_BE.Services.GoogleService;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Models;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class NotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<NotificationResponseDTO> GetNotification(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var data = await _unitOfWork.Notification.GetNotification(userId, page, pageSize);

                var notificationDTOs = _mapper.Map<List<NotificationDTO>>(data.Item1);
                int totalCount = data.Item2;

                NotificationResponseDTO notification = new NotificationResponseDTO
                {
                    Notifications = notificationDTOs,
                    TotalCount = totalCount
                };

                return notification;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting notifications for user {UserId}", userId);
                throw new InternalServerException("An error occurred while retrieving notifications.");
            }
        }
    }
}