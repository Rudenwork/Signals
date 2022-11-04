using Microsoft.EntityFrameworkCore;
using Signals.App.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SignalsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(Signals))));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();