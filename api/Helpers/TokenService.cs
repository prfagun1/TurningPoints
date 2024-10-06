using entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Helpers
{
    public class TokenService
    {
        public string? Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        private readonly IConfiguration _configuration;
        private string _privateKey;
        private string _issuer;
        private string _expirationTimeInMinutes;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            try
            {
                _privateKey = _configuration["Jwt:Key"] ?? throw new ArgumentException("The configuration 'Jwt:Key' cannot be null.");
                _issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentException("The configuration 'Jwt:Issuer' cannot be null.");
                _expirationTimeInMinutes = _configuration["Jwt:ExpirationTimeInMinutes"] ?? throw new ArgumentException("The configuration 'Jwt:ExpirationTimeInMinutes' cannot be null.");
            }
            catch (Exception e)
            {
                string message = "JWT parameters not found, check JWT:Key, JWT:Issuer, and JWT:ExpirationTimeInMinutes.";
                _logger.LogCritical(e, message);
                throw new Exception(message);
            }
        }

        public string CreateToken(User user)
        {
            if (user.Roles.Count == 0)
                throw new Exception("The user must have at least one role.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_privateKey);
            List<Claim> claimList = new List<Claim>();

            var expirationDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_expirationTimeInMinutes));

            foreach (var role in user.Roles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, role));
            }

            if (string.IsNullOrEmpty(user.Username))
                throw new Exception("The user has an empty or null Username field.");

            claimList.Add(new Claim(ClaimTypes.Role, "Public"));
            claimList.Add(new Claim(ClaimTypes.Name, user.Username));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = expirationDate,
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
