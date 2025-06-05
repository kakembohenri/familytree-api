namespace familytree_api.Services
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; }

        // Constructor for success
        public ApiResponse(T data, string message = "Request successful", int statusCode = 200)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        // Constructor for failure
        public ApiResponse(IEnumerable<string> errors, string message = "Request failed", int statusCode = 404)
        {
            Success = false;
            Errors = errors;
            Message = message;
            Data = default;
            StatusCode = statusCode;
        }
    }
}
