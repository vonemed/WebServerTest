using System.Reflection;
using SharedLibrary.Packets;
using SharedLibrary.Packets.ClientToServer;
using WebServer.Services;

namespace WebServer.Static;

//A static class, to map all packets and their respective handlers for server to use
public static class NetworkManager
{
    public static readonly Dictionary<string, Type> MapPackets = new ();
    public static readonly Dictionary<string, PacketHandler> MapHandlers = new ();
    public static void Init(IServiceProvider serviceProvider)
    {
        //Packets
        //Find the right assembly
        var assemblyShared = Assembly.GetAssembly(typeof(IClientToServerPacket));
        //Get all the types in it
        var allTypesShared = assemblyShared.GetTypes();

        //Process all the type and map them in the dictionary
        foreach (Type type in allTypesShared.Where(x => !x.IsAbstract && x.IsClass && x.GetInterfaces().Contains(typeof(IClientToServerPacket))))
        {
            var command = serviceProvider.GetService<IClientToServerPacket>();

            if(command == null) continue;

            if (!MapPackets.ContainsValue(type))
            {
                MapPackets.Add(type.FullName,type);
                Console.WriteLine($"Packet key -> {type.FullName} | Packet type -> {type}");
            }
        }

        if (MapPackets.Count > 0)
        {
            Console.WriteLine("All packets are stored");
        } else Console.WriteLine("No packets to store!");
        
        //Handlers
        //Find the right assembly
        var assemblyServer = Assembly.GetAssembly(typeof(PacketHandler));
        //Get all the types in it
        var allTypesServer = assemblyServer.GetTypes();
        
        //Process all the type and map them in the dictionary
        foreach (Type type in allTypesServer.Where(x => !x.IsAbstract && x.IsClass))
        {
            var command = (PacketHandler) serviceProvider.GetService(type);
            
            if(command == null) continue;

            if (!MapHandlers.ContainsKey(command.Key) && !MapHandlers.ContainsValue(command))
            {
                MapHandlers.Add(command.Key ,command);
                Console.WriteLine($"Handler key -> {command.Key} | Handler type -> {command}");
            }
        }

        if (MapHandlers.Count > 0)
        {
            Console.WriteLine("All handlers are stored");
        } else Console.WriteLine("No handlers to store!");
    }
}