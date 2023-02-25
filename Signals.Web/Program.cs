using Signals.Web.Settings;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Map("settings", () => app.Configuration.Get<AppSettings>());

app.Run();
