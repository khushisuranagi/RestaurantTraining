namespace RestaurantTraining.Application.Common.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string>? ValidationErrors { get; set; }
    }
}