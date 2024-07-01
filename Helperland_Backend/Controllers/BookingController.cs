using Helperland.Entity.Model;
using Helperland.Repository.BookingService;
using Microsoft.AspNetCore.Mvc;

namespace Helperland.Controllers
{
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        #region ZipCodeCheck
        [HttpGet, Route("ZipCodeCheck")]
        public ResponseModel<bool> ZipCodeCheck(string ZipCode)
        {
            try
            {
                ResponseModel<bool> responseModel = _bookingService.ZipCodeCheck(ZipCode);
                return responseModel;
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
