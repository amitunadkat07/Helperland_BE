using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using Microsoft.AspNetCore.Http;

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
        public ResponseModel<bool> ZipCodeCheck(string ZipCode)
        {
            try
            {
                if (string.IsNullOrEmpty(ZipCode))
                {
                    return new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Please enter Zip Code value",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }
                else
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
                        return new ResponseModel<bool>
                        {
                            Data = true,
                            IsSuccess = true,
                            Message = "success",
                            StatusCode = StatusCodes.Status200OK,
                        };
                    }
                    else
                    {
                        return new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = "We are not providing service in this area.",
                            StatusCode = StatusCodes.Status404NotFound,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion
    }
}
