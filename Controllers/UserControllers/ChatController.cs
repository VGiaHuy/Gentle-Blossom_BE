using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequestDTO request)
        {
            var chatRoom = await _chatService.CreateChatRoomAsync(request.ChatRoomName, request.IsGroup, request.userCreate);

            return Ok(new API_Response<ChatRoomDTO>
            {
                Success = true,
                Message = "Tạo phòng chat thành công!",
                Data = chatRoom
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetChatRoom(int chatRoomId)
        {
            var chatRoom = await _chatService.GetChatRoomAsync(chatRoomId);

            if (chatRoom == null)
                return Ok(new API_Response<ChatRoomDTO>
                {
                    Success = false,
                    Message = "Không tìm thấy phòng chat!",
                    Data = null
                });

            return Ok(new API_Response<ChatRoomDTO>
            {
                Success = true,
                Message = "Lấy thông tin phòng chat thành công!",
                Data = chatRoom
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChatRoomName(UpdateChatRoomNameDTO request)
        {
            await _chatService.UpdateChatRoomNameAsync(request.ChatRoomId, request.NewName);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Cập nhật tên phòng chat thành công!",
                Data = null
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChatRoom(int chatRoomId)
        {
            await _chatService.DeleteChatRoomAsync(chatRoomId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Xóa phòng chat thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToChatRoom(AddUserToChatRoomDTO request)
        {
            await _chatService.AddUserToChatRoomAsync(request.ChatRoomId, request.UserId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Thêm thành viên thành công!",
                Data = null
            });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromChatRoom(RemoveUserFromChatRoomDTO request)
        {
            await _chatService.RemoveUserFromChatRoomAsync(request.ChatRoomId, request.ParticipantId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Xóa thành viên thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageRequestDTO request)
        {
            await _chatService.SendMessageAsync(request.ChatRoomId, request.SenderId, request.Content, request.Attachment);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Gửi tin nhắn thành công!",
                Data = null
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            await _chatService.DeleteMessageAsync(messageId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Xóa tin nhắn thành công!",
                Data = null
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int chatRoomId)
        {
            var messages = await _chatService.GetMessagesAsync(chatRoomId);

            return Ok(new API_Response<List<MessageDTO>> 
            {
                Success = true,
                Message = "Lấy tin nhắn thành công!",
                Data = messages
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChatRooms([FromQuery] int userId)
        {
            var chatRooms = await _chatService.GetUserInChatRoomsAsync(userId);

            return Ok(new API_Response<List<ChatRoomDTO>>
            {
                Success = true,
                Message = "Lấy danh sách phòng chat thành công!",
                Data = chatRooms
            });
        }
    }
}
