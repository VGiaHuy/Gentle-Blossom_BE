using AutoMapper;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.GoogleService;
using GentleBlossom_BE.Services.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GentleBlossom_BE.Services.UserServices
{
    public class PregnancyCareService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly ChatService _chatService;

        public PregnancyCareService(IUnitOfWork unitOfWork, IMapper mapper, ChatService chatService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _chatService = chatService; 
        }

        public async Task<bool> ConnectMessageAsync(ConnectMessageDTO connectMessage)
        {
            try
            {
                var chatRoomName = "Tư vấn sức khỏe";

                var post = await _unitOfWork.Post.GetByIdAsync(connectMessage.PostId);
                if (post == null)
                {
                    throw new BadRequestException("Không tìm thấy bài viết");
                }

                var chatRoom = await _chatService.CreateChatRoomAsync(chatRoomName, false, connectMessage.ExpertId);

                await _chatService.AddUserToChatRoomAsync(chatRoom.ChatRoomId, post.PosterId);

                return true;
            }
            catch(Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while connecting the message.", ex);
            }
        }
    }
}
