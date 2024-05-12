using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Datapia.Services
{
    public class JwtService
    {
        private readonly string _secret = ConfigurationManager.AppSettings.Get("API_SECRET");
        private readonly string _expDate = ConfigurationManager.AppSettings.Get("API_EXPIRATIONINMINUTES");
        private readonly string _expDateR = ConfigurationManager.AppSettings.Get("API_EXPIRATIONINMINUTES_REFRESH");
        private readonly string _expDate_OTP = ConfigurationManager.AppSettings.Get("API_EXPIRATIONINMINUTES_OTP");
        public string GenerateToken(string user_code, string modules, string application_code)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("user_code", user_code),
                    new Claim("Token", "true"),
                    new Claim("modules", modules),
                    new Claim("application_code", application_code),
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
        public string Generate_refreshToken(string user_code, string modules, string application_code)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                 new Claim("user_code", user_code),
                 new Claim("Token", "false"),
                 new Claim("modules", modules),
                 new Claim("application_code", application_code),
                 }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDateR)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}