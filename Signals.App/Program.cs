using Microsoft.EntityFrameworkCore;
using Signals.App.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SignalsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(SignalsContext))));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();