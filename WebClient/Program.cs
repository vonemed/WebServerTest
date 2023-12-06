// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

var uri = new Uri("ws://193.124.129.94:5143");
var clientSocket = new ClientWebSocket();
var source = new CancellationTokenSource();
source.CancelAfter(5000);
        
Console.WriteLine("Connecting...");
// [Obsolete("Do not use this in Production code!!!",false)]
// static void NEVER_EAT_POISON_Disable_CertificateValidation()
// {
//     // Disabling certificate validation can expose you to a man-in-the-middle attack
//     // which may allow your encrypted message to be read by an attacker
//     // https://stackoverflow.com/a/14907718/740639
//     ServicePointManager.ServerCertificateValidationCallback =
//         delegate (
//             object s,
//             X509Certificate certificate,
//             X509Chain chain,
//             SslPolicyErrors sslPolicyErrors
//         ) {
//             return true;
//         };
// }
//
// NEVER_EAT_POISON_Disable_CertificateValidation();

// clientSocket.Options.UseDefaultCredentials = true;
await clientSocket.ConnectAsync(uri, source.Token);

if(clientSocket.State == WebSocketState.Open) Console.WriteLine("Connected!");