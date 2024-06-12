using Helperland.Entity.Model;

namespace Helperland.Repository.Interface
{
    public interface IUserService
    {
        public UserDataModel Login(LoginModel user);

        public UserDataModel Signup(UserModel user);

        public ResetPass ForgotPass(ResetPass user);

        public ResetPass ResetPassLink(ResetPass user);

        public ResetPass ResetPass(ResetPass user);

        public List<UserDataModel> GetUsers();
    }
}
