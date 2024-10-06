using entities;
using repository.Interface;

namespace api.Helpers
{
    public class LoginHelper
    {

        private readonly IUnitOfWork _iuw;

        public LoginHelper(IUnitOfWork iuw)
        {
            _iuw = iuw;
        }

        public User? GetPermissions(LoginViewModel login)
        {


            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return null;


            if (!_iuw.Login.GetValidLogin(login.Email, login.Password))
                return null;
            

            User user = new User
            {
                Username = login.Email
            };

            
            user.Roles.Add(_iuw.Login.GetPermission(login.Email).ToString());

            return user;


        }

    }
}
