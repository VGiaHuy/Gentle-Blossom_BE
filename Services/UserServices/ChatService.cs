using AutoMapper;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Helpers;
using GentleBlossom_BE.Services.GoogleService;
using GentleBlossom_BE.Services.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GentleBlossom_BE.Services.UserServices
{
    public class ChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IWebHostEnvironment _environment;

        private readonly IMapper _mapper;

        private readonly GoogleDriveService _googleDriveService;

        public ChatService(IHubContext<ChatHub> hubContext, IWebHostEnvironment environment, IUnitOfWork unitOfWork, IMapper mapper, GoogleDriveService googleDriveService)
        {
            _hubContext = hubContext;
            _environment = environment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleDriveService = googleDriveService;
        }

        // Tạo phòng chat
        public async Task<ChatRoomDTO> CreateChatRoomAsync(string? chatRoomName, bool isGroup, int userCreate)
        {
            try
            {
                var chatRoom = new ChatRoom
                {
                    ChatRoomName = chatRoomName,
                    IsGroup = isGroup,
                    CreatedAt = DateTime.UtcNow
                };

                // Lưu phòng chat
                var createdRoom = await _unitOfWork.ChatRoom.CreateChatRoomAsync(chatRoom);
                //await _unitOfWork.SaveChangesAsync();

                // Tạo chatcode
                createdRoom.ChatCode = RoomCodeHelper.GenerateChatCode(createdRoom.ChatRoomName, createdRoom.ChatRoomId);
                _unitOfWork.ChatRoom.Update(createdRoom);

                // Thêm người dùng vào phòng
                await _unitOfWork.ChatRoomUser.AddAsync(new ChatRoomUser
                {
                    ChatRoomId = createdRoom.ChatRoomId,
                    ParticipantId = userCreate,
                    JoinedAt = DateTime.UtcNow
                });

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
            catch (Exception ex)
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
            catch (Exception ex)
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
        public async Task SendMessageAsync(int chatRoomId, int senderId, string? content, List<IFormFile>? attachments)
        {
            try
            {
                var message = new Message
                {
                    ChatRoomId = chatRoomId,
                    SenderId = senderId,
                    Content = content,
                    HasAttachment = attachments != null && attachments.Any(file => file.Length > 0),
                    SentAt = DateTime.UtcNow
                };

                // Lưu tin nhắn
                var createdMessage = await _unitOfWork.Message.CreateMessageAsync(message);
                await _unitOfWork.SaveChangesAsync();

                var mediaItems = new List<MessageAttachment>();

                // Xử lý file đính kèm
                if (attachments != null && attachments.Any(file => file.Length > 0))
                {
                    const long maxFileSize = 100 * 1024 * 1024; // Giới hạn 100MB
                    var uploadTasks = attachments.Select(async file =>
                    {
                        try
                        {
                            // Kiểm tra kích thước file
                            if (file.Length > maxFileSize)
                            {
                                throw new BadRequestException($"File {file.FileName} vượt quá kích thước cho phép (100MB)");
                            }

                            // Kiểm tra ContentType
                            if (string.IsNullOrEmpty(file.ContentType))
                            {
                                throw new BadRequestException($"File {file.FileName} thiếu ContentType");
                            }

                            // Xác định loại media
                            string mediaType = file.ContentType switch
                            {
                                string ct when ct.StartsWith("image/") => "Image",
                                string ct when ct.StartsWith("video/") => "Video",
                                string ct when ct == "application/pdf" => "PDF",
                                _ => throw new BadRequestException($"Loại file không được hỗ trợ: {file.ContentType}")
                            };

                            // Upload file lên Google Drive
                            var fileUrl = await _googleDriveService.UploadFileAsync(file, MediaType.Message);

                            return new MessageAttachment
                            {
                                MessageId = createdMessage.MessageId,
                                FileName = file.FileName,
                                FileUrl = fileUrl,
                                FileType = mediaType,
                                FileSize = file.Length // Lấy kích thước file
                            };
                        }
                        catch (Exception ex)
                        {
                            // Log lỗi nhưng không làm gián đoạn các file khác
                            Console.WriteLine($"Lỗi upload file {file.FileName}: {ex.Message}");
                            return null; // Trả về null để lọc sau
                        }
                    }).ToList();

                    // Chờ tất cả upload hoàn tất
                    mediaItems = (await Task.WhenAll(uploadTasks))
                        .Where(m => m != null)
                        .ToList();

                    // Lưu các attachment vào DB
                    foreach (var media in mediaItems)
                    {
                        await _unitOfWork.MessageAttachment.AddAsync(media);
                    }
                    // Lưu tất cả thay đổi trong một lần
                    await _unitOfWork.SaveChangesAsync();
                }

                var user = await _unitOfWork.UserProfile.GetByIdAsync(senderId);

                if (user == null)
                {
                    throw new InternalServerException("Đã xảy ra lỗi ở Server");
                }
                // Chuẩn bị dữ liệu cho SignalR
                var senderAvatarUrl = user.AvatarUrl; // Lấy từ DB hoặc dịch vụ user
                var senderName = user.FullName; // Lấy từ DB hoặc dịch vụ user
                var messageId = createdMessage.MessageId;
                var mediaList = mediaItems.Select(m => new
                {
                    m.FileUrl,
                    MediaType = m.FileType,
                    m.FileName
                }).ToList();

                // Gửi tin nhắn qua SignalR
                await _hubContext.Clients.Group($"Room_{chatRoomId}")
                    .SendAsync("ReceiveMessage", messageId, senderId, content, mediaList, createdMessage.SentAt, senderAvatarUrl, senderName);
            }
            catch (Exception e)
            {
                throw new InternalServerException("Lỗi: " + e.Message);
            }
        }

        // Xóa tin nhắn
        public async Task DeleteMessageAsync(int messageId, int chatRoomId)
        {
            try
            {
                var message = await _unitOfWork.Message.GetByIdAsync(messageId);

                if (message == null)
                    throw new NotFoundException("Message not found.");

                _unitOfWork.Message.Delete(message);

                // Xóa tệp đính kèm nếu có
                bool attachments = await _unitOfWork.MessageAttachment.DeleteByMessageId(messageId);

                if (!attachments)
                {
                    throw new InternalServerException("Đã xảy ra lỗi xóa tệp đính kèm");
                }

                await _unitOfWork.SaveChangesAsync();

                // Thông báo qua SignalR
                await _hubContext.Clients.Group($"Room_{chatRoomId}")
                        .SendAsync("MessageDeleted", messageId);
            }
            catch (Exception e)
            {
                throw new InternalServerException("Lỗi: " + e.Message);
            }
        }

        // Lấy danh sách tin nhắn
        public async Task<List<MessageDTO>> GetMessagesAsync(int chatRoomId)
        {
            var message = await _unitOfWork.Message.GetMessagesByChatRoomAsync(chatRoomId);

            return message;
        }

        public async Task<List<ChatRoomDTO>> GetUserInChatRoomsAsync(int userId)
        {
            var chatRooms = await _unitOfWork.ChatRoom.GetChatRoomsByUserAsync(userId);

            var chatRoomDTO = _mapper.Map<List<ChatRoomDTO>>(chatRooms);

            return chatRoomDTO;
        }

        public async Task JoinChatRoom(string chatCode, int userId)
        {
            try
            {
                var data = RoomCodeHelper.DecodeChatCode(chatCode);
                var checkChatRoom = await _unitOfWork.ChatRoom.GetByIdAsync(data.chatRoomId);
                var checkUserExist = await _unitOfWork.ChatRoomUser.CheckUserExistInChatRoom(data.chatRoomId ,userId);

                if (checkChatRoom == null || data.chatRoomName != checkChatRoom.ChatRoomName)
                {
                    throw new BadRequestException("Phòng chat không tồn tại! Vui lòng kiểm tra lại Chat Code");
                }

                if (checkUserExist == true)
                {
                    throw new BadRequestException("Bạn đã tham gia phòng chat này trước đó!");
                }

                await AddUserToChatRoomAsync(data.chatRoomId, userId);
            }
            catch (Exception e)
            {
                throw new InternalServerException(e.Message);
            }

        }
    }
}
