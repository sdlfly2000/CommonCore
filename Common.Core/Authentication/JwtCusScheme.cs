using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.Core.Authentication
{
    public static class JwtCusScheme
    {

        public static AuthenticationBuilder AddJwtCusScheme(this AuthenticationBuilder builder, JWTOptions jwtOpt)
        {
            return builder.AddScheme<JwtBearerOptions, JwtCustomHandler>(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
                var secKey = new SymmetricSecurityKey(keyBytes);
                option.TokenValidationParameters = new()
                {
                    ValidIssuer = jwtOpt.Issuer,

                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secKey
                };
            });
        }
    }

    public class JwtCustomHandler : JwtBearerHandler
    {
        public JwtCustomHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var remoteIpAdress = Context.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var auth = Request.Headers.Authorization.ToString();

            var token = GetTokenBody(auth) as JObject;

            if (token == null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (remoteIpAdress != token[ClaimTypes.Uri]?.Value<string>()
                || userAgent != token[ClaimTypes.UserData]?.Value<string>())
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            return base.HandleAuthenticateAsync();
        }

        #region Private Methods

        private object? GetTokenBody(string auth)
        {
            if (auth.Length < 8)
            {
                return default;
            }

            var tokenBase64 = auth
                                .Substring("Bearer ".Length)
                                .Trim()
                                .Split('.');

            if (tokenBase64.Length != 3)
            {
                return default;
            }

            var tokenBodyBlob = ConvertBase64ToObject(tokenBase64[1]); // 1-> Jwt body
            var tokenBodyJson = Encoding.UTF8.GetString(tokenBodyBlob);

            return JsonConvert.DeserializeObject(tokenBodyJson);
        }

        private byte[] ConvertBase64ToObject(string base64)
        {
            if (base64.Length % 4 != 0)
                base64 += new String('=', 4 - base64.Length % 4);

            return Convert.FromBase64String(base64);
        }

        #endregion
    }
}
