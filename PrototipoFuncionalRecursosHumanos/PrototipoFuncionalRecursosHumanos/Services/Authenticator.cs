using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrototipoFuncionalRecursosHumanos.Services
{
    public class Authenticator
    {
        public Authenticator()
        {
        }

        public void CrearToken(string correo, HttpResponse response)
        {
            var builder = WebApplication.CreateBuilder();
            var secretKey = builder.Configuration["SecretKey:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.Email] = correo,
            };
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "PrototipoRecursosHumanos",
                Audience = "PrototipoRecursosHumanos",
                Claims = claims,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JsonWebTokenHandler();
            handler.SetDefaultTimesOnTokenCreation = false;
            var token = handler.CreateToken(descriptor);
            // Guardar el token en una cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(24),
            };
            response.Cookies.Append("token", token, cookieOptions);
        }

        public string ValidarToken(HttpRequest request)
        {
            var token = request.Cookies["token"];
            var builder = WebApplication.CreateBuilder();
            var secretKey = builder.Configuration["SecretKey:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "PrototipoRecursosHumanos",
                ValidAudience = "PrototipoRecursosHumanos",
                IssuerSigningKey = securityKey
            };

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var emailClaim = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email);
                return emailClaim.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
