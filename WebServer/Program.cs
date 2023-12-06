using SharedLibrary.Packets;
using SharedLibrary.Packets.ClientToServer;
using WebServer.Services;
using WebServer.Static;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<LoginService>();
        builder.Services.AddTransient<IClientToServerPacket, LoginRequestPacket>();

        // builder.Services.AddSwaggerGen();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        var websocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };

        app.UseWebSockets(websocketOptions);
        // app.UseLoginMiddleware()

        app.Use(async (context, next) =>
        {
            NetworkManager.Init(app.Services);
            await next();
        });

        app.Run();

        // app.Run();
    }
}

