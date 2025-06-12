using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using GentleBlossom_BE.Data.DTOs.UserDTOs;

using GentleBlossom_BE.Data.Repositories.Interface;

namespace GentleBlossom_BE.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Gửi tin nhắn tới phòng chat
        public async Task SendMessage(int chatRoomId, int senderId, string content, List<string> mediaUrls)
        {
            try
            {
                // Kiểm tra đầu vào
                if (chatRoomId <= 0 || senderId <= 0)
                {
                    throw new ArgumentException("Invalid chatRoomId or senderId");
                }

                // Lấy thông tin người gửi
                var user = await _unitOfWork.UserProfile.GetByIdAsync(senderId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                var senderAvatarUrl = user.AvatarUrl ?? "default-avatar.png";
                var senderName = user.FullName ?? "Unknown";

                // Chuyển đổi mediaUrls thành mediaList
                var mediaList = mediaUrls?.Select(url => new MessageMediaDTO
                {
                    MediaUrl = url,
                    MediaType = GetMediaType(url),
                    FileName = Path.GetFileName(url)
                }).ToList() ?? new List<MessageMediaDTO>();

                // Gửi thông báo tới tất cả client trong phòng
                await Clients.Group($"Room_{chatRoomId}").SendAsync(
                    "ReceiveMessage",
                    senderId,
                    content,
                    mediaList,
                    DateTime.UtcNow, // Hoặc lấy từ backend nếu cần
                    senderAvatarUrl,
                    senderName
                );
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                throw; // Ném lại để client nhận lỗi (nếu cần)
            }
        }

        private string GetMediaType(string url)
        {
            var extension = Path.GetExtension(url).ToLower();
            return extension switch
            {
                string ext when ext.Contains("jpg") || ext.Contains("png") || ext.Contains("jpeg") => "Image",
                string ext when ext.Contains("mp4") || ext.Contains("mov") => "Video",
                string ext when ext.Contains("pdf") => "PDF",
                _ => "File"
            };
        }

        // Tham gia phòng chat
        public async Task JoinRoom(int chatRoomId, int userId)
        {
            try
            {
                if (chatRoomId <= 0)
                    throw new ArgumentException("Invalid chatRoomId");
                if (userId <= 0)
                    throw new ArgumentException("Invalid userId");

                // Thêm client vào nhóm
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Room_{chatRoomId}");
                // Gửi thông báo đến tất cả client trong nhóm
                await Clients.OthersInGroup($"Room_{chatRoomId}").SendAsync("UserJoined", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in JoinRoom: {ex.Message}");
                throw;
            }
        }

        // Rời phòng chat
        public async Task LeaveRoom(int chatRoomId, int userId)
        {
            try
            {
                if (chatRoomId <= 0)
                    throw new ArgumentException("Invalid chatRoomId");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Room_{chatRoomId}");
                await Clients.OthersInGroup($"Room_{chatRoomId}").SendAsync("UserLeft", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LeaveRoom: {ex.Message}");
                throw;
            }
        }

        // Thông báo tin nhắn bị xóa
        public async Task DeleteMessage(int chatRoomId, int messageId)
        {
            try
            {
                if (chatRoomId <= 0)
                    throw new ArgumentException("Invalid chatRoomId");
                if (messageId <= 0)
                    throw new ArgumentException("Invalid messageId");
                await Clients.Group($"Room_{chatRoomId}")
                    .SendAsync("MessageDeleted", messageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteMessage: {ex.Message}");
                throw;
            }
        }
    }
}
