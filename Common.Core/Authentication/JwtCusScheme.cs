using Common.Core.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AuthService.Middlewares
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
        private readonly IMemoryCache _memoryCache;

        public JwtCustomHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IMemoryCache memoryCache)
            : base(options, logger, encoder, clock)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var userAgent = Request.Headers.UserAgent.ToString();

            var token = GetTokenBody(Request.Headers.Authorization.ToString()) as JObject;

            if (token == null)
            {
                return AuthenticateResult.NoResult();
            }

            var cacheJwtKey = CreateKey(
                token[ClaimTypes.NameIdentifier]?.Value<string>(),
                token["exp"]?.Value<string>());

            if (!IsActicatedOrEmpty(cacheJwtKey))
            {
                return AuthenticateResult.NoResult();
            }

            if (userAgent != token[ClaimTypes.UserData]?.Value<string>())
            {
                return AuthenticateResult.NoResult();
            }

            var authResult = await base.HandleAuthenticateAsync();

            if (authResult.Succeeded)
            {
                var isValid = _memoryCache.GetOrCreate(cacheJwtKey,
                    (entry) =>
                    {
                        entry.SetValue(true);
                        entry.SetSlidingExpiration(TimeSpan.FromDays(1));
                        return true;
                    });
            }

            Context.Items.Add("CacheJwtKey", cacheJwtKey);

            return authResult;
        }

        #region Private Methods

        private bool IsActicatedOrEmpty(string cacheKey)
        {
            if (_memoryCache.TryGetValue(cacheKey, out var isValid))
            {
                if ((bool)isValid! == false)
                {
                    return false;
                }
            }

            return true;
        }

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
                base64 += new string('=', 4 - base64.Length % 4);

            return Convert.FromBase64String(base64);
        }

        private string CreateKey(string? userId, string? timeStamp)
        {
            return string.Join('|',
                new string?[] { userId, timeStamp }
                    .Where(e => !e.IsNullOrEmpty())
                    .ToArray());
        }

        #endregion
    }
}
