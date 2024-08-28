using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;


IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 11000);
Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    socketListener.Bind(ipEndPoint);
    socketListener.Listen(10);

    while (true)
    {
        Console.Clear();
        Console.WriteLine("En attente d'une connexion...");

        Socket handler = socketListener.Accept();

        //Ajouter code pour lancer la partie

    }
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}