namespace Helperland.Entity.Model
{
    public class AddressDataModel
    {
        public int? AddressId { get; set; }

        public string? Email { get; set; }

        public string? Street { get; set; }

        public string? House { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? Contact { get; set; }

        public bool IsError { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
