using GatoLockAPI.Hubs;
using GatoLockAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddSignalR();

builder.Services.AddSingleton<SolicitacoesQueueService>();
builder.Services.AddSingleton<MensagemService>();
builder.Services.AddHostedService<ProcessamentoSolicitacoesService>();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapGrpcService<MensagensGrpcService>();
app.MapHub<SolicitacoesHub>("/hubs/solicitacoes");

app.MapControllers();

app.Run();