using Helperland.Entity.Model;

namespace Helperland.Repository.Interface
{
    public interface IUserService
    {
        public UserDataModel login(LoginModel user);

        public UserDataModel signup(UserModel user);

        public ResetPass forgotPass(ResetPass user);

        public ResetPass resetPassLink(ResetPass user);

        public ResetPass resetPass(ResetPass user);

        public List<UserDataModel> getusers();
    }
}
