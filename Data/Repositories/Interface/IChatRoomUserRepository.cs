﻿using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IChatRoomUserRepository : IGenericRepository<ChatRoomUser>
    {
        Task RemoveUserFromChatRoomAsync(int chatRoomId, int participantId);
        Task<List<ChatRoomUser>> GetChatRoomUsersAsync(int userId);
        Task<bool> CheckUserExistInChatRoom(int chatRoom, int userId);
        Task<int> GetUseIdByChatRoomIdAsync(int chatRoomId, int expertId);
        Task<bool> AreParticipantsInPrivateChatRoomAsync(int participantId1, int participantId2, CancellationToken cancellationToken = default);
        Task<List<UsersInChatRoomDTO>> GetUsersInChatRoom(int chatRoomId);
        Task<int?> GetExistingPrivateChatRoomIdAsync(int userId1, int userId2);
    }
}
