using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Game
    {
        Tableau tableau;

        public Game()
        {
            this.tableau = new Tableau();
        }

        public void StartGame(Socket socket)
        {
            bool bateauChoisiServer = ChoisirBateau(socket);

            if (bateauChoisiServer)
            {
                while (true)
                {
                    string win = JouerTour(socket);

                    if (win == "notWin")
                        AdversaireJouer(socket);
                    else
                        break;
                  
                }

                Console.Clear();
                Console.WriteLine("Vous avez gagné!!");
                RestartGame(socket);           
            }
        }

        public void RestartGame(Socket socket)
        {
            Console.WriteLine("Voulez-vous rejouer une partie. Oui(o) ou Non (n)");
            string choix = "";

            do
            {
                Console.WriteLine("Veuillez choisir un une position ou tirer dans le tableau");
                choix = Console.ReadLine();
                choix.ToLower();
            }
            while (choix != "o" && choix != "n");

            if (choix == "o")
            {
                Tir tir = new Tir(1);
                tir.status = "newGame";
                tableau.EnvoyerTir(tir, socket);
                StartGame(socket);

            }
            else
            {
                Tir tir = new Tir(1);
                tir.status = "stop";
                tableau.EnvoyerTir(tir, socket);
            }
        }

        public bool ChoisirBateau(Socket socket)
        {
            AfficherJeux();
            //Choisi sont bateau
            bool bateauChoisi = tableau.ChoixBateau();

            AfficherJeux();
            //Fait choisir le serveur
            EnvoyerChoixBateau(bateauChoisi, socket);

            //Attend que le serveur aille choisi
            Console.WriteLine("Votre adversaire choisi l'emplacement du bateau");
            return RecevoirChoixBateau(socket);
        }

        public string JouerTour(Socket socket)
        {
            Tir tir = tableau.ChoixTir();
            tir.status = "toCheck";

            tableau.EnvoyerTir(tir, socket);

            tir = tableau.RecevoirTir(socket);

            if(tir.status == "win")
            {
                tableau.AjoutTir(tir);
                AfficherJeux();
                return "win";
            }

            AfficherJeux();

            if (tir.status == "check")
            {
                tableau.AjoutTir(tir);
                tir.status = "changeTour";
            }

            AfficherJeux();

            tableau.EnvoyerTir(tir, socket);

            return "notWin";
        }

        public void AdversaireJouer(Socket socket)
        {
            Tir tir = tableau.RecevoirTir(socket);

            if (tir.status == "toCheck")
            {
                tir = tableau.VerificationTir(tir);
                bool gagnant = tableau.VerifierGagnant();
                if (gagnant)
                    tir.status = "win";
                    
                tableau.EnvoyerTir(tir, socket);
            }
            AfficherJeux();


            bool nextTour = false;
            while (nextTour != true)
            {
                tir = tableau.RecevoirTir(socket);
                if (tir.status == "changeTour")
                    nextTour = true;
            }
        }

        public void AfficherJeux()
        {
            Console.Clear();
            Console.WriteLine("Tableau Joueur");
            Console.WriteLine();
            tableau.AffichageTableauJoueur();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Tableau Adversaire");
            Console.WriteLine();
            tableau.AffichageTableauAdversaire();
            Console.WriteLine();
        }

        public void EnvoyerChoixBateau(bool bateauChoisi, Socket socket)
        {
            string jsonBool = Serialiser.SerialiseBoolToJson(bateauChoisi);
            byte[] bytes = Encoding.ASCII.GetBytes(jsonBool);
            socket.Send(bytes);
        }

        public bool RecevoirChoixBateau(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            return Serialiser.DeserialiseBoolFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }
    }
}
