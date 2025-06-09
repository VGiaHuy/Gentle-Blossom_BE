using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace GentleBlossom_BE.Services.Hubs
{
    public class ChatHub : Hub
    {
        // Gửi tin nhắn tới phòng chat
        public async Task SendMessage(int chatRoomId, int senderId, string content, string? attachmentUrl)
        {
            try
            {
                if (chatRoomId <= 0)
                    throw new ArgumentException("Invalid chatRoomId");
                if (senderId <= 0)
                    throw new ArgumentException("Invalid senderId");

                await Clients.Group($"Room_{chatRoomId}")
                    .SendAsync("ReceiveMessage", senderId, content, attachmentUrl, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                throw;
            }
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
                await Clients.OthersInGroup($"Room_{chatRoomId}")
                    .SendAsync("UserJoined", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in JoinRoom: {ex.Message}");
                throw;
            }
        }

        // Rời phòng chat
        public async Task LeaveRoom(int chatRoomId)
        {
            try
            {
                if (chatRoomId <= 0)
                    throw new ArgumentException("Invalid chatRoomId");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Room_{chatRoomId}");
                await Clients.Caller.SendAsync("UserLeft", Context.UserIdentifier ?? "Unknown");
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
