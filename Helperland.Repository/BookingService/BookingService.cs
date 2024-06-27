using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperland.Repository.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly HelperlandContext _context;

        public BookingService(HelperlandContext context)
        {
            _context = context;
        }

        #region ZipCodeCheck
        public bool ZipCodeCheck(string ZipCode)
        {
            try
            {
                List<User>? users = (from user in _context.Users
                                     where user.RoleId == 3 && user.ZipCode == ZipCode
                                     select new User
                                     {
                                         UserId = user.UserId,
                                         Email = user.Email
                                     }).ToList();
                if (users.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
