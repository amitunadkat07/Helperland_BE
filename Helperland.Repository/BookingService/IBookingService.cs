using Helperland.Entity.Model;

namespace Helperland.Repository.BookingService
{
    public interface IBookingService
    {
        public ResponseModel<bool> ZipCodeCheck(string ZipCode);
    }
}
