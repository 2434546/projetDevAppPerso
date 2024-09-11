using Client;
using System.Net;
using System.Net.Sockets;
using System.Text;

byte[] bytes = new byte[1024];
string message = string.Empty;

try
{
    var client = new Clients();
    // Connect to a remote device.
    Console.Write("Entrée l'adresse du server : ");
    string ipServer = Console.ReadLine();
    
    client.Connect(ipServer);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}