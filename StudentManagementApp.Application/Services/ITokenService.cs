using System.Security.Claims;

namespace StudentManagementApp.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
