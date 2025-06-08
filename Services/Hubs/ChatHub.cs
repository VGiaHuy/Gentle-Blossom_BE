using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace GentleBlossom_BE.Services.Hubs
{
    public class ChatHub : Hub
    {
        // Gửi tin nhắn tới phòng chat
        public async Task SendMessage(int chatRoomId, int senderId, string content, string? attachmentUrl)
        {
            await Clients.Group($"Room_{chatRoomId}")
                .SendAsync("ReceiveMessage", senderId, content, attachmentUrl, DateTime.UtcNow);
        }

        // Tham gia phòng chat
        public async Task JoinRoom(int chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Room_{chatRoomId}");
        }

        // Rời phòng chat
        public async Task LeaveRoom(int chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Room_{chatRoomId}");
        }

        // Thông báo tin nhắn bị xóa
        public async Task DeleteMessage(int chatRoomId, int messageId)
        {
            await Clients.Group($"Room_{chatRoomId}")
                .SendAsync("MessageDeleted", messageId);
        }
    }
}
