using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using AutoMapper;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.UserServices
{
    public class ChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;


        public ChatService(IHubContext<ChatHub> hubContext, IWebHostEnvironment environment, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _hubContext = hubContext;
            _environment = environment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Tạo phòng chat
        public async Task<ChatRoomDTO> CreateChatRoomAsync(string? chatRoomName, bool isGroup, List<int> participantIds)
        {
            try
            {
                var chatRoom = new ChatRoom
                {
                    ChatRoomName = isGroup ? chatRoomName : null,
                    IsGroup = isGroup,
                    CreatedAt = DateTime.UtcNow
                };

                // Lưu phòng chat
                var createdRoom = await _unitOfWork.ChatRoom.CreateChatRoomAsync(chatRoom);

                // Thêm người dùng vào phòng
                foreach (var participantId in participantIds)
                {
                    await _unitOfWork.ChatRoomUser.AddAsync(new ChatRoomUser
                    {
                        ChatRoomId = createdRoom.ChatRoomId,
                        ParticipantId = participantId,
                        JoinedAt = DateTime.UtcNow
                    });
                }

                var createdRoomDto = new ChatRoomDTO
                {
                    ChatRoomId = createdRoom.ChatRoomId,
                    ChatRoomName = createdRoom.ChatRoomName,
                    IsGroup = createdRoom.IsGroup,
                    CreatedAt = createdRoom.CreatedAt,
                };

                await _unitOfWork.SaveChangesAsync();

                return createdRoomDto;
            }
            catch(Exception ex)
            {
                // Xử lý lỗi nếu cần
                throw new InternalServerException("Error creating chat room: " + ex.Message);
            }
        }

        // Lấy thông tin phòng chat
        public async Task<ChatRoomDTO?> GetChatRoomAsync(int chatRoomId)
        {
            try
            {
                var chatRoom = await _unitOfWork.ChatRoom.GetByIdAsync(chatRoomId);

                if (chatRoom == null)
                    return null;

                var chatRoomDto = _mapper.Map<ChatRoomDTO>(chatRoom);

                return chatRoomDto;
            }
            catch(Exception ex)
            {
                // Xử lý lỗi nếu cần
                throw new InternalServerException("Error retrieving chat room: " + ex.Message);
            }
        }

        // Cập nhật tên phòng chat
        public async Task UpdateChatRoomNameAsync(int chatRoomId, string newName)
        {
            var chatRoom = await _unitOfWork.ChatRoom.GetByIdAsync(chatRoomId);

            if (chatRoom == null || !chatRoom.IsGroup)
                throw new Exception("Chat room not found or not a group.");

            chatRoom.ChatRoomName = newName;

            _unitOfWork.ChatRoom.Update(chatRoom);
            await _unitOfWork.SaveChangesAsync();
        }

        // Xóa phòng chat
        public async Task DeleteChatRoomAsync(int chatRoomId)
        {
            var chatRoom = await _unitOfWork.ChatRoom.GetByIdAsync(chatRoomId);

            if (chatRoom == null)
                throw new Exception("Chat room not found.");

            _unitOfWork.ChatRoom.Delete(chatRoom);

            // Xóa tất cả người dùng trong phòng chat (ChatRoomUser)

            await _unitOfWork.SaveChangesAsync();
        }

        // Thêm người dùng vào phòng chat
        public async Task AddUserToChatRoomAsync(int chatRoomId, int participantId)
        {
            var chatRoom = await _unitOfWork.ChatRoom.GetByIdAsync(chatRoomId);

            if (chatRoom == null)
                throw new Exception("Chat room not found.");

            await _unitOfWork.ChatRoomUser.AddAsync(new ChatRoomUser
            {
                ChatRoomId = chatRoomId,
                ParticipantId = participantId,
                JoinedAt = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();

            // Thông báo qua SignalR
            await _hubContext.Clients.Group($"Room_{chatRoomId}")
                .SendAsync("UserJoined", participantId);
        }

        // Xóa người dùng khỏi phòng chat
        public async Task RemoveUserFromChatRoomAsync(int chatRoomId, int participantId)
        {
            await _unitOfWork.ChatRoomUser.RemoveUserFromChatRoomAsync(chatRoomId, participantId);
            await _unitOfWork.SaveChangesAsync();

            // Thông báo qua SignalR
            await _hubContext.Clients.Group($"Room_{chatRoomId}")
                .SendAsync("UserLeft", participantId);
        }

        // Gửi tin nhắn
        public async Task SendMessageAsync(int chatRoomId, int senderId, string? content, IFormFile? attachment)
        {
            var message = new Message
            {
                ChatRoomId = chatRoomId,
                SenderId = senderId,
                Content = content,
                HasAttachment = attachment != null,
                SentAt = DateTime.UtcNow
            };

            // Lưu tin nhắn
            var createdMessage = await _unitOfWork.Message.CreateMessageAsync(message);

            string? attachmentUrl = null;
            if (attachment != null)
            {
                // Lưu file vào thư mục wwwroot/uploads
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }

                attachmentUrl = $"/Uploads/{fileName}";

                // Lưu thông tin tệp đính kèm
                await _unitOfWork.MessageAttachment.AddAsync(new MessageAttachment
                {
                    MessageId = createdMessage.MessageId,
                    FileName = attachment.FileName,
                    FileUrl = attachmentUrl,
                    FileType = attachment.ContentType,
                    FileSize = attachment.Length / 1024, // Chuyển sang KB
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();

            // Gửi tin nhắn qua SignalR
            await _hubContext.Clients.Group($"Room_{chatRoomId}")
                .SendAsync("ReceiveMessage", senderId, content, attachmentUrl, createdMessage.SentAt);
        }

        // Xóa tin nhắn
        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _unitOfWork.Message.GetByIdAsync(messageId);

            if (message == null)
                throw new NotFoundException("Message not found.");

            _unitOfWork.Message.Delete(message);

            // Xóa tệp đính kèm nếu có

            await _unitOfWork.SaveChangesAsync();

            // Thông báo qua SignalR
            await _hubContext.Clients.Group($"Room_{message.ChatRoomId}")
                    .SendAsync("MessageDeleted", messageId);
        }

        // Lấy danh sách tin nhắn
        public async Task<List<MessageDTO>> GetMessagesAsync(int chatRoomId)
        {
            var message = await _unitOfWork.Message.GetMessagesByChatRoomAsync(chatRoomId);

            var messageDto = _mapper.Map<List<MessageDTO>>(message);

            return messageDto;
        }

        public async Task<List<ChatRoomDTO>> GetUserInChatRoomsAsync(int userId)
        {
            var chatRooms = await _unitOfWork.ChatRoom.GetChatRoomsByUserAsync(userId);

            var chatRoomDTO = _mapper.Map<List<ChatRoomDTO>>(chatRooms);

            return chatRoomDTO;
        }
    }
}
