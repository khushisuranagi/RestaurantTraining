using RestaurantTraining.Blazor.Components;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.CertificateManagement;
using RestaurantTraining.Blazor.Services.DashboardManagement;
using RestaurantTraining.Blazor.Services.LessonManagement;
using RestaurantTraining.Blazor.Services.ModuleManagement;
using RestaurantTraining.Blazor.Services.QuizManagement;
using RestaurantTraining.Blazor.Services.LearnerQuizManagement;
using RestaurantTraining.Blazor.Services.LearnerDashboardManagement;

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
builder.Services.AddScoped<RestaurantTraining.Blazor.Services.PeopleManagement.IPeopleService,
    RestaurantTraining.Blazor.Services.PeopleManagement.PeopleService>();
builder.Services.AddScoped<RestaurantTraining.Blazor.Services.ProfileManagement.IProfileService,
    RestaurantTraining.Blazor.Services.ProfileManagement.ProfileService>();


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
