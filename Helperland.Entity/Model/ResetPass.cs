namespace Helperland.Entity.Model
{
    public class ResetPass
    {
        public string? Email { get; set; }

        public string? ResetKey { get; set; }

        public string? Date { get; set; }

        public string? Password { get; set; }

        public string? ConfPassword { get; set; }

        public bool IsError { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
