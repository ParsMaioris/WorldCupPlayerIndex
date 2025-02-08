using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class LocalNetworkJwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalNetworkJwtTokenService(IOptions<JwtSettings> options, IHttpContextAccessor httpContextAccessor)
    {
        _settings = options.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken()
    {
        var context = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not available");
        var remoteIp = context.Connection.RemoteIpAddress ?? throw new Exception("Remote IP not available");
        if (!IsLocalNetwork(remoteIp))
            throw new ForbiddenException("Token issuance is restricted to local network requests");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.SecretKey);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "LocalNetworkUser"),
            new Claim("ip", remoteIp.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }

    private bool IsLocalNetwork(IPAddress remoteIp)
    {
        if (IPAddress.IsLoopback(remoteIp))
            return true;
        if (remoteIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            var bytes = remoteIp.GetAddressBytes();
            if (bytes[0] == 10)
                return true;
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                return true;
            if (bytes[0] == 192 && bytes[1] == 168)
                return true;
        }
        return false;
    }
}