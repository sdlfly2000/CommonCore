using Common.Core.Shared.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
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
        private const int LiveTimeMin = 60;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IOptions<JWTOptions> _jwtOptions;

        public JwtCustomHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IMemoryCacheService memoryCacheService, IOptions<JWTOptions> jwtOptions)
            : base(options, logger, encoder)
        {
            _memoryCacheService = memoryCacheService;
            _jwtOptions = jwtOptions;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var cancellationToken = CancellationToken.None;
            var userAgent = Request.Headers.UserAgent.ToString();
            var token = GetTokenBody(Request.Headers.Authorization.ToString()) as JObject;
            var liveTimeMin = _jwtOptions.Value.LiveTimeMin == 0 ? LiveTimeMin : _jwtOptions.Value.LiveTimeMin;

            if (token == null)
            {
                return AuthenticateResult.NoResult();
            }

            var cacheJwtKey = CreateKey(
                token[ClaimTypes.NameIdentifier]?.Value<string>(),
                token["exp"]?.Value<string>());

            if ((await IsActicatedOrEmpty(cacheJwtKey, cancellationToken).ConfigureAwait(false)) == false)
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
                _ = await _memoryCacheService.Upsert<bool>(cacheJwtKey, true, TimeSpan.FromHours(liveTimeMin), CancellationToken.None).ConfigureAwait(false);
            }

            Context.Items.Add("CacheJwtKey", cacheJwtKey);

            return authResult;
        }

        #region Private Methods

        private async Task<bool> IsActicatedOrEmpty(string cacheKey, CancellationToken token)
        {
            var result = await _memoryCacheService.GetValue<bool>(cacheKey, token).ConfigureAwait(false);
            if (result.Success)
            {
                if (result.CachedValue == false)
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
                    .Where(e => !string.IsNullOrEmpty(e))
                    .ToArray());
        }

        #endregion
    }
}
