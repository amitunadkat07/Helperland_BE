namespace Helperland.Entity.Model
{
    public class ProfileDataModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set;}

        public string? Email { get; set; }

        public string? Contact { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Language { get; set; }

        public bool IsError { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
