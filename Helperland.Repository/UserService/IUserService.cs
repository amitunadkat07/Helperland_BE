using Helperland.Entity.Model;

namespace Helperland.Repository.Interface
{
    public interface IUserService
    {
        public ResponseModel<UserDataModel> Login(LoginModel user);

        public ResponseModel<UserDataModel> Signup(UserModel user);

        public ResponseModel<ResetPass> ForgotPass(ResetPass user);

        public ResponseModel<ResetPass> ResetPassLink(ResetPass user);

        public ResponseModel<ResetPass> ResetPass(ResetPass user);

        public ResponseModel<List<UserDataModel>> GetUsers();

        public ProfileDataModel GetProfile(string email);

        /*public ResponseModel<ProfileDataModel> GetProfile(string email);*/

        public ProfileDataModel UpdateProfile(ProfileDataModel profile);

        public PasswordModel UpdatePassword(PasswordModel password);

        public AddressDataModel CreateAddress(AddressDataModel address);

        public AddressDataModel UpdateAddress(AddressDataModel address);

        public List<AddressDataModel> GetAddressByUser(string email);

        public AddressDataModel GetAddressById(int id);

        public bool DeleteAddress(int id);
    }
}
