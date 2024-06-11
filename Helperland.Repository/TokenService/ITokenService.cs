using Helperland.Entity.Model;

namespace Helperland.Repository.TokenService
{
    public interface ITokenService
    {
        public string GenerateJWTAuthetication(UserDataModel userData);
    }
}
