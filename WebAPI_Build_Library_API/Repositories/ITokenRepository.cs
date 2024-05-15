using Microsoft.AspNetCore.Identity;

namespace WebAPI_Build_Library_API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
