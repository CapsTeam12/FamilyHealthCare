using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ZoomToken
    {
        public string Token { get; set; }
        public ZoomToken(string zoomApiKey, string zoomApiSecret)
        {
            DateTime Expiry = DateTime.UtcNow.AddMinutes(5);
            string ApiKey = zoomApiKey;
            string ApiSecret = zoomApiSecret;

            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiSecret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create Token

            // Header
            var header = new JwtHeader(credentials);

            //Payload
            var payload = new JwtPayload
            {
                {"iss",ApiKey },
                {"exp",ts }
            };

            var setToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(setToken);
            this.Token = token;
        }
    }
}
