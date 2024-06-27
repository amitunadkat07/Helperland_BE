using Helperland.Repository.BookingService;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ZipCodeCheck(string ZipCode)
        {
            try
            {
                if (string.IsNullOrEmpty(ZipCode))
                {
                    return BadRequest();
                }
                else
                {
                    if (_bookingService.ZipCodeCheck(ZipCode))
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
