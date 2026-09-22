namespace RestaurantTraining.Application.Common.Interfaces
{
    // Simple "prompt in, text out" AI call. Used by the test endpoint.
    public interface IAiTextGenerator
    {
        Task<string?> GenerateTextAsync(string prompt, CancellationToken cancellationToken);
    }
}
