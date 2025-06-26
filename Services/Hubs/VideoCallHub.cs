// GentleBlossom_BE/Hubs/VideoCallHub.cs
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace GentleBlossom_BE.Services.Hubs
{
    public class VideoCallHub : Hub
    {
        public static readonly ConcurrentDictionary<string, List<(int UserId, string ConnectionId)>> RoomParticipants = new();
        private readonly IUnitOfWork _unitOfWork;

        public VideoCallHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<string> GetUserName(int userId)
        {
            var user = await _unitOfWork.UserProfile.GetByIdAsync(userId);
            return user?.FullName ?? "Unknown";
        }

        public async Task JoinRoom(string roomId, string userId)
        {
            if (!int.TryParse(userId, out int parsedUserId))
            {
                throw new HubException("Invalid userId provided.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            RoomParticipants.AddOrUpdate(roomId,
                new List<(int, string)> { (parsedUserId, Context.ConnectionId) },
                (key, oldValue) =>
                {
                    if (!oldValue.Any(p => p.ConnectionId == Context.ConnectionId))
                    {
                        oldValue.Add((parsedUserId, Context.ConnectionId));
                    }
                    return oldValue;
                });
            await Clients.Group(roomId).SendAsync("UserJoined", Context.ConnectionId);
            Console.WriteLine($"User {parsedUserId} with ConnectionId {Context.ConnectionId} joined room {roomId}");
        }

        public async Task LeaveRoom(string roomId)
        {
            int userId = RoomParticipants[roomId]?.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId).UserId ?? 0;
            if (RoomParticipants.ContainsKey(roomId))
            {
                RoomParticipants[roomId].RemoveAll(p => p.ConnectionId == Context.ConnectionId);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            Console.WriteLine($"User {userId} with ConnectionId {Context.ConnectionId} left room {roomId}");
        }

        public async Task<List<UsersInChatRoomDTO>> GetUsersInRoom(string roomId)
        {
            if (!RoomParticipants.ContainsKey(roomId)) return new List<UsersInChatRoomDTO>();

            var result = new List<UsersInChatRoomDTO>();
            foreach (var p in RoomParticipants[roomId])
            {
                result.Add(new UsersInChatRoomDTO
                {
                    Id = p.UserId,
                    Name = await GetUserName(p.UserId),
                    ConnectionId = p.ConnectionId
                });
            }
            Console.WriteLine($"GetUsersInRoom for room {roomId}: {JsonSerializer.Serialize(result)}");
            return result;
        }

        public async Task SendOffer(string targetConnectionId, string offer)
        {
            if (string.IsNullOrEmpty(targetConnectionId))
            {
                throw new HubException("Target connection ID is null or empty.");
            }
            try
            {
                await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
                Console.WriteLine($"Sent Offer from {Context.ConnectionId} to {targetConnectionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending Offer to {targetConnectionId}: {ex.Message}");
                throw new HubException($"Failed to send Offer to {targetConnectionId}: {ex.Message}");
            }
        }

        public async Task SendAnswer(string targetConnectionId, string answer)
        {
            if (string.IsNullOrEmpty(targetConnectionId))
            {
                throw new HubException("Target connection ID is null or empty.");
            }
            try
            {
                await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
                Console.WriteLine($"Sent Answer from {Context.ConnectionId} to {targetConnectionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending Answer to {targetConnectionId}: {ex.Message}");
                throw new HubException($"Failed to send Answer to {targetConnectionId}: {ex.Message}");
            }
        }

        public async Task SendIceCandidate(string targetConnectionId, string candidate)
        {
            if (string.IsNullOrEmpty(targetConnectionId))
            {
                throw new HubException("Target connection ID is null or empty.");
            }
            try
            {
                await Clients.Client(targetConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
                Console.WriteLine($"Sent ICE candidate from {Context.ConnectionId} to {targetConnectionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending ICE candidate to {targetConnectionId}: {ex.Message}");
                throw new HubException($"Failed to send ICE candidate to {targetConnectionId}: {ex.Message}");
            }
        }
    }
}
