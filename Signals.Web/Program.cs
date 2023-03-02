using Signals.Web.Settings;

var builder = WebApplication.CreateBuilder(args);

var proxySection = builder.Configuration.GetSection("ReverseProxy");
proxySection["Clusters:default:Destinations:app:Address"] = builder.Configuration["ApiBaseAddress"];

builder.Services.AddReverseProxy()
    .LoadFromConfig(proxySection);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapReverseProxy();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Map("settings", () => app.Configuration.Get<AppSettings>());

app.Run();
