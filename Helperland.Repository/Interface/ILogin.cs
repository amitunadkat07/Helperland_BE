using Helperland.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperland.Repository.Interface
{
    public interface Ilogin
    {
        public bool login(LoginModel user);

        public bool signup(UserModel user);
    }
}
