var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles(); 
app.UseStaticFiles();  

app.MapGet("/api/status", () =>
{
    return Results.Ok("Servidor online!");
});

app.Run();