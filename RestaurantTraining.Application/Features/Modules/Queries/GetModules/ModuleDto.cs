namespace RestaurantTraining.Application.Features.Modules.Queries.GetModules
{
    // A simple, safe shape of a Module that we send back to the client.
    public class ModuleDto
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
