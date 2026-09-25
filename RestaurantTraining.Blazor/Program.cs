using RestaurantTraining.Web.Components;
using RestaurantTraining.Web.Services;
using RestaurantTraining.Web.Services.CertificateManagement;
using RestaurantTraining.Web.Services.DashboardManagement;
using RestaurantTraining.Web.Services.LearnerDashboardManagement;
using RestaurantTraining.Web.Services.LearnerQuizManagement;
using RestaurantTraining.Web.Services.LessonManagement;
using RestaurantTraining.Web.Services.ModuleManagement;
using RestaurantTraining.Web.Services.QuizManagement;
using RestaurantTraining.Web.Services.SettingsManagement;

var builder = WebApplication.CreateBuilder(args);  //instance

builder.Services.AddRazorComponents()              //adds components
    .AddInteractiveServerComponents();

// read from appsettings.json
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]!;


builder.Services.AddScoped(sp => new HttpClient   //api uses httpclient
{
    BaseAddress = new Uri(apiBaseUrl)
});


builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ILessonManagementService, LessonManagementService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<ILearnerDashboardService,LearnerDashboardService>();
builder.Services.AddScoped<ILearnerQuizService, LearnerQuizService>();
builder.Services.AddScoped<RestaurantTraining.Web.Services.PeopleManagement.IPeopleService,
    RestaurantTraining.Web.Services.PeopleManagement.PeopleService>();
builder.Services.AddScoped<RestaurantTraining.Web.Services.ProfileManagement.IProfileService,
    RestaurantTraining.Web.Services.ProfileManagement.ProfileService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<RestaurantTraining.Web.Services.LearnerExplore.IExploreService,
    RestaurantTraining.Web.Services.LearnerExplore.ExploreService>();
builder.Services.AddScoped<RestaurantTraining.Web.Services.AssistantManagement.IAssistantService,
    RestaurantTraining.Web.Services.AssistantManagement.AssistantService>();
builder.Services.AddScoped<RestaurantTraining.Web.Services.LeaderboardManagement.ILeaderboardService,
    RestaurantTraining.Web.Services.LeaderboardManagement.LeaderboardService>();

var app = builder.Build();   //web app instance


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
