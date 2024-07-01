namespace Helperland.Entity.Model
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }

        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }
    }
}
