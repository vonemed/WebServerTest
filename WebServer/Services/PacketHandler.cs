using SharedLibrary.Packets;

namespace WebServer.Services;

//Default base class for all the handlers to process packets
public abstract class PacketHandler<T> : PacketHandler where T : IClientToServerPacket
{
    public override void HandlePacket(object packet)
    {
        Handle((T)packet);
    }

    public abstract void Handle(T packet);
}

public abstract class PacketHandler
{
    public abstract string Key { get; set; }
    
    public abstract void HandlePacket(object packet);
}