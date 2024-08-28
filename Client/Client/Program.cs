using System.Net;
using System.Net.Sockets;
using System.Text;

byte[] bytes = new byte[1024];
string message = string.Empty;

try
{
    // Connect to a remote device.
    Console.Write("Entrée l'adresse du server : ");
    string ipServer = Console.ReadLine();
    IPAddress ipAdress = IPAddress.Parse(ipServer);
    IPEndPoint ipEndPoint = new IPEndPoint(ipAdress, 11000);
    Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    sender.Connect(ipEndPoint);
    Console.WriteLine("Connexion établie");

    //Ajouter la truc qui lance la game

    // Release the socket.
    sender.Shutdown(SocketShutdown.Both);
    sender.Close();
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}