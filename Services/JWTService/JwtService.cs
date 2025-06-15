using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GentleBlossom_BE.Data.DTOs.UserDTOs;

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
    }
}
