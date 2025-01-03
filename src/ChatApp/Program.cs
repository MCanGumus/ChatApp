using ChatApp;
using ChatApp.Features.SendMessage;
using Shared;
using Shared.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddSignalR();

builder.Services.AddScoped<ChatHub>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonService(typeof(ChatAppAssembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapHub<ChatHub>("/chatHub");

app.AddMessageGroupEndpointExt();
app.UseHttpsRedirection();

app.Run();