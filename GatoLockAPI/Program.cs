using GatoLockAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<MensagemService>();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.Run();