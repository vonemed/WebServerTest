using SharedLibrary.Models;
using SharedLibrary.Packets.ClientToServer;
using WebServer.DAL;

namespace WebServer.Services;

public class LoginService : PacketHandler<LoginRequestPacket>
{
    public override string Key { get; set; } = typeof(LoginRequestPacket).FullName;
    
    public override void Handle(LoginRequestPacket packet)
    {
        Console.WriteLine("New player login");

        var player = new Player
        {
            Name = packet.Username
        };
        
        PlayerDatabase.AddPlayer(player);
        PlayerDatabase.DisplayPlayers();
    }
}