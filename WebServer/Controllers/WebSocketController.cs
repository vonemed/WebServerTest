using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Packets;
using SharedLibrary.Packets.ClientToServer;
using WebServer.DAL;
using WebServer.Services;
using WebServer.Static;


namespace WebServer.Controllers;

[ApiController]
public class WebSocketController : ControllerBase
{
    // private readonly RequestDelegate _next;
    //
    // private readonly HttpContext _context;
    private LoginService _loginService;
    
    public WebSocketController(LoginService loginService)
    {
        // _context = context;
        _loginService = loginService;
    }
    
    [Route("/ws")]
    public async Task InvokeAsync()
    {
        WriteRequestParams(HttpContext);
            
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine("WebSocketConnected");
            
            //Process requested socket
            await ReceiveMessage(webSocket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Message received");
                    Console.WriteLine($"Message: {message}");

                    var receivedMessage = JsonConvert.DeserializeObject<PacketContainer>(message);
                    
                    if(receivedMessage != null) HandlePacket(receivedMessage);
                    else
                    {
                        Console.WriteLine("Can't deserialize message into a container! :(");
                    }
                    
                    return;
                }
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Message closed");
                    return;
                }
            });
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            // await _next(context);
        }
    }
    
    //Receive data from client websocket
    private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 4];

        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), CancellationToken.None);
            
            handleMessage(result, buffer);
        }
    }

    //Display parameters of a request in a console
    private void WriteRequestParams(HttpContext context)
    {
        Console.WriteLine("Request method: " + context.Request.Method);
        Console.WriteLine("Request Protocol: " + context.Request.Protocol);

        if (context.Request.Headers != null)
        {
            foreach (var header in context.Request.Headers)
            {
                Console.WriteLine($"---> {header.Key} : {header.Value}");
            }
        }
    }
    
    private void HandlePacket(PacketContainer container)
    {
        Console.WriteLine("Handling the container...");
        Console.WriteLine($"Container-> {container}");
        
        try
        {
            var packet = NetworkManager.MapPackets[container.Key];
            Console.WriteLine($"Found the packet type! -> {packet}");
            
            var packetHandler = NetworkManager.MapHandlers[container.Key];
        
            Console.WriteLine($"Found the packet handler! -> {packetHandler}");

            var data = JsonConvert.DeserializeObject(container.Data, packet);
            packetHandler.HandlePacket(data);
        }
        catch (Exception e)
        {
            Console.WriteLine("CAN'T FIND THE PACKET IN MAP");
            Console.WriteLine(e);
            throw;
        }
    }
}