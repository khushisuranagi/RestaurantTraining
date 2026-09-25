namespace RestaurantTraining.Web.Models;

// One optional (unassigned) module in the Explore list.
public class ExploreModuleModel
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CoverImageData { get; set; }
    public string? CoverImageContentType { get; set; }
}
