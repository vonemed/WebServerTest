namespace SharedLibrary.Packets.ClientToServer
{
    public sealed class LoginRequestPacket : IClientToServerPacket
    {
        public string Username { get; set; } = "";
    }
}