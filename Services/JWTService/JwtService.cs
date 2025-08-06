using GentleBlossom_BE.Data.DTOs.UserDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GentleBlossom_BE.Services.JWTService
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<JwtResponseDTO> CreateTokenUser(string name)
        {
            try
            {
                var issuer = _configuration["JwtConfig:Issuer"];
                var audience = _configuration["JwtConfig:Audience"];
                var key = _configuration["JwtConfig:Key"];
                var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
                var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Name, name),
                        new Claim(ClaimTypes.Role, "User")
                    }),
                    Expires = tokenExpiryTimeStamp,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(securityToken);

                return new JwtResponseDTO
                {
                    AccessToken = accessToken,
                    ExpiresIn = tokenValidityMins * 60
                };
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và trả về phản hồi thích hợp
                throw new InvalidOperationException("Error creating JWT token", ex);
            }
        }

        //public async Task<LoginResponseModel?> CreateTokenAdmin(string NameNV)
        //{
        //    try
        //    {
        //        var issuer = _configuration["JwtConfig:Issuer"];
        //        var audience = _configuration["JwtConfig:Audience"];
        //        var key = _configuration["JwtConfig:Key"];
        //        var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        //        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new[]
        //            {
        //                new Claim(JwtRegisteredClaimNames.Name, NameNV!),
        //                new Claim(ClaimTypes.Role, "Admin")
        //            }),
        //            Expires = tokenExpiryTimeStamp,
        //            Issuer = issuer,
        //            Audience = audience,
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256Signature),
        //        };

        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //        var accessToken = tokenHandler.WriteToken(securityToken);

        //        return new LoginResponseModel
        //        {
        //            AccessToken = accessToken,
        //            SDT = NameNV,
        //            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log lỗi và trả về phản hồi thích hợp
        //        throw new InvalidOperationException("Error creating JWT token", ex);
        //    }
        //}

        public async Task<string> CreateOtpToken(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var otpValidityMins = 5; // OTP hết hạn sau 5 phút
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(otpValidityMins);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("otp", otp)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var otpToken = tokenHandler.WriteToken(securityToken);

            return otpToken;
        }

        public bool ValidateOtpToken(string token, string inputEmail, string inputOtp)
        {
            var key = _configuration["JwtConfig:Key"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateLifetime = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var otp = principal.FindFirst("otp")?.Value;

                return email == inputEmail && otp == inputOtp;
            }
            catch
            {
                return false;
            }
        }

    }
}
