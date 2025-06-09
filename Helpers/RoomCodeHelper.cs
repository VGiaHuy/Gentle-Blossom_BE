using System.Text;

namespace GentleBlossom_BE.Helpers
{
    public static class RoomCodeHelper
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const char Separator = '_';

        /// <summary>
        /// Tạo chatCode từ tên phòng chat và ID phòng chat
        /// </summary>
        /// <param name="chatRoomName">Tên phòng chat</param>
        /// <param name="chatRoomId">ID phòng chat</param>
        /// <returns>ChatCode đã được mã hóa Base62</returns>
        public static string GenerateChatCode(string chatRoomName, int chatRoomId)
        {
            // Tạo chuỗi gốc: [chatRoomName]_[chatRoomId]
            string originalString = $"{chatRoomName}_{chatRoomId}";

            // Mã hóa Base62
            return EncodeBase62(originalString);
        }

        /// <summary>
        /// Giải mã chatCode để lấy thông tin phòng chat
        /// </summary>
        /// <param name="chatCode">ChatCode đã được mã hóa</param>
        /// <returns>Tuple chứa tên phòng chat và ID phòng chat</returns>
        public static (string chatRoomName, int chatRoomId) DecodeChatCode(string chatCode)
        {
            try
            {
                // Giải mã Base62
                string decodedString = DecodeBase62(chatCode);

                // Tìm vị trí của dấu phân cách cuối cùng
                int lastSeparatorIndex = decodedString.LastIndexOf(Separator);

                if (lastSeparatorIndex == -1)
                    throw new InvalidOperationException("ChatCode không đúng định dạng");

                // Tách tên phòng chat và ID
                string chatRoomName = decodedString.Substring(0, lastSeparatorIndex);
                string chatRoomIdStr = decodedString.Substring(lastSeparatorIndex + 1);

                if (string.IsNullOrWhiteSpace(chatRoomName))
                    throw new InvalidOperationException("Tên phòng chat không hợp lệ");

                if (!int.TryParse(chatRoomIdStr, out int chatRoomId) || chatRoomId <= 0)
                    throw new InvalidOperationException("ID phòng chat không hợp lệ");

                return (chatRoomName, chatRoomId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể giải mã chatCode: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mã hóa chuỗi thành Base62
        /// </summary>
        private static string EncodeBase62(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            // Convert byte array to big integer
            var value = new System.Numerics.BigInteger(bytes.Concat(new byte[] { 0 }).ToArray());

            if (value == 0)
                return Base62Chars[0].ToString();

            var result = new StringBuilder();
            while (value > 0)
            {
                value = System.Numerics.BigInteger.DivRem(value, 62, out var remainder);
                result.Insert(0, Base62Chars[(int)remainder]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Giải mã Base62 thành chuỗi gốc
        /// </summary>
        private static string DecodeBase62(string input)
        {
            var value = new System.Numerics.BigInteger(0);

            foreach (char c in input)
            {
                int index = Base62Chars.IndexOf(c);
                if (index == -1)
                    throw new ArgumentException($"Ký tự không hợp lệ: {c}");

                value = value * 62 + index;
            }

            byte[] bytes = value.ToByteArray();

            // Remove the extra zero byte we added during encoding
            if (bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
            {
                Array.Resize(ref bytes, bytes.Length - 1);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của chatCode
        /// </summary>
        /// <param name="chatCode">ChatCode cần kiểm tra</param>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        public static bool IsValidChatCode(string chatCode)
        {
            try
            {
                var (_, _) = DecodeChatCode(chatCode);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
