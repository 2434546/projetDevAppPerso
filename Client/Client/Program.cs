using Client;
using System.Net;
using System.Net.Sockets;
using System.Text;

byte[] bytes = new byte[1024];
string message = string.Empty;

/*try
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
    Game game = new Game();
    game.StartGame(sender);

    // Release the socket.
    sender.Shutdown(SocketShutdown.Both);
    sender.Close();
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}*/



Tableau tableau1 = new Tableau();

tableau1.AffichageTableauJoueur();
Console.WriteLine();
Console.WriteLine();
tableau1.AffichageTableauAdversaire();
Console.WriteLine();

tableau1.ChoixBateau();

while (true)
{


    Console.Clear();
    tableau1.AffichageTableauJoueur();
    Console.WriteLine();
    Console.WriteLine();
    tableau1.AffichageTableauAdversaire();
    Console.WriteLine();

    Tir tir = tableau1.ChoixTir();
    tableau1.VerificationTir(tir);


}



