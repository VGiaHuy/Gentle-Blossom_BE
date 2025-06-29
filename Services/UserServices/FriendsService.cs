using AutoMapper;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class FriendsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ChatService _chatService;

        public FriendsService(IUnitOfWork unitOfWork, IMapper mapper, ChatService chatService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _chatService = chatService;
        }

        public async Task<List<ExpertProfileDTO>> GetAllExperts()
        {
            try
            {
                return await _unitOfWork.Expert.GetAllExperts();
            }
            catch(Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }
        public async Task<ExpertProfileDTO> GetExpertById(int expertId)
        {
            try
            {
                return await _unitOfWork.Expert.GetExpertsProfile(expertId);
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }

        public async Task<int?> SendMessage(RequestConnectChat connectChat)
        {
            try
            {
                int? chatRoomId = 0;
                var expert = await _unitOfWork.Expert.GetByIdAsync(connectChat.ExpertId);

                // kiểm tra nếu cuộc trò chuyện đã tồn tại thì không tạo cuộc trò chuyện mới
                var connection = await _unitOfWork.ChatRoomUser.AreParticipantsInPrivateChatRoomAsync(expert.UserId, connectChat.UserId);

                if (!connection)
                {
                    var chatRoomName = "Tư vấn sức khỏe";
                    var chatRoom = await _chatService.CreateChatRoomAsync(chatRoomName, false, connectChat.UserId);
                    chatRoomId = chatRoom.ChatRoomId;
                    await _chatService.AddUserToChatRoomAsync(chatRoom.ChatRoomId, expert.UserId);
                }
                else
                {
                    chatRoomId = await _unitOfWork.ChatRoomUser.GetExistingPrivateChatRoomIdAsync(expert.UserId, connectChat.UserId);     
                }

                await _unitOfWork.SaveChangesAsync();

                return chatRoomId;
            }
            catch(Exception e)
            {
                throw new InternalServerException(e.Message);
            }
        }
    }
}
