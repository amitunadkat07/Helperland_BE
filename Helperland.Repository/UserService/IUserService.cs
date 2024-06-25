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

        public ProfileDataModel GetProfile(string email);

        public ProfileDataModel UpdateProfile(ProfileDataModel profile);

        public PasswordModel UpdatePassword(PasswordModel password);
    }
}
