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

        public AddressDataModel CreateAddress(AddressDataModel address);

        public AddressDataModel UpdateAddress(AddressDataModel address);

        public List<AddressDataModel> GetAddressByUser(string email);

        public AddressDataModel GetAddressById(int id);

        public bool DeleteAddress(int id);
    }
}
