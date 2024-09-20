﻿using System;
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
        private int gridSize;
        Tableau tableau;

        public Game()
        {
        }

        public void StartGame(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            gridSize = int.Parse(Encoding.ASCII.GetString(bytes, 0, bytesRec));

            this.tableau = new Tableau(gridSize);

            bool bateauChoisiServer = ChoisirBateau(socket);
            string win = "notWin";

            if (bateauChoisiServer)
            {
                while (true)
                {

                    string status;
                    if (win == "notWin")
                        status = JouerTour(socket);
                    else
                        break;

                    if (status == "continu")
                        win = AdversaireJouer(socket);
                    else
                        break;

                }

                RestartGame(socket);           
            }
        }

        private void RestartGame(Socket socket)
        {
            Console.WriteLine("Voulez-vous rejouer une partie. Oui(o) ou Non (n)");
            string? choix;

            do
            {
                choix = Console.ReadLine();
                if(choix != null)
                    choix.ToLower();
            }
            while (choix != "o" && choix != "n");

            if (choix == "o")
            {
                Tir? tir = new Tir(1);
                tir.status = "newGame";
                tableau.EnvoyerTir(tir, socket);

                tableau.ClearTableau();
                StartGame(socket);

            }
            else
            {
                Tir tir = new Tir(1);
                tir.status = "stop";
                tableau.EnvoyerTir(tir, socket);
                
            }
        }

        private bool ChoisirBateau(Socket socket)
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

        private string JouerTour(Socket socket)
        {
            Tir? tir = tableau.ChoixTir();
            tir.status = "toCheck";

            tableau.EnvoyerTir(tir, socket);

            tir = tableau.RecevoirTir(socket);

            if (tir != null)
            {
                if (tir.status == "win")
                {
                    tableau.AjoutTir(tir);
                    Console.Clear();
                    Console.WriteLine("Vous avez gagné!!");
                    return "win";
                }
            }

            AfficherJeux();
            if (tir != null)
            {
                if (tir.status == "check")
                {
                    tableau.AjoutTir(tir);
                    tir.status = "changeTour";
                }
            }

            AfficherJeux();

            if(tir != null)
                tableau.EnvoyerTir(tir, socket);

            return "continu";
        }

        private string AdversaireJouer(Socket socket)
        {
            Tir? tir = tableau.RecevoirTir(socket);
            if(tir != null)
            {
                if (tir.status == "toCheck")
                {
                    tir = tableau.VerificationTir(tir);
                    bool gagnant = tableau.VerifierGagnant();
                    if (gagnant)
                        tir.status = "win";

                    tableau.EnvoyerTir(tir, socket);

                    if (tir.status == "win")
                    {
                        Console.Clear();
                        Console.WriteLine("Votre adversaire a gagnée");
                        return "";
                    }

                    AfficherJeux();


                    bool nextTour = false;
                    while (nextTour != true)
                    {
                        tir = tableau.RecevoirTir(socket);
                        if(tir != null)
                        {
                            if (tir.status == "changeTour")
                                nextTour = true;
                            if (tir.status == "newGame" || tir.status == "stop")
                                return "";
                        }
                    }

                }
            }
            return "notWin";
        }

        private void AfficherJeux()
        {
            Console.Clear();
            AfficherLegende();
            Console.WriteLine();
            Console.WriteLine("Votre Tableau");
            Console.WriteLine();
            tableau.AffichageTableauJoueur();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Tableau de l'Adversaire");
            Console.WriteLine();
            tableau.AffichageTableauAdversaire();
            Console.WriteLine();
        }

        private void AfficherLegende()
        {
            Console.WriteLine("LÉGENDE :");
            Console.WriteLine("XX = Tir dans l'eau");
            Console.WriteLine("BB = Position du bateau");
            Console.WriteLine("BT = Partie de bateau touché");
        }

        private void EnvoyerChoixBateau(bool bateauChoisi, Socket socket)
        {
            string jsonBool = Serialiser.SerialiseBoolToJson(bateauChoisi);
            byte[] bytes = Encoding.ASCII.GetBytes(jsonBool);
            socket.Send(bytes);
        }

        private bool RecevoirChoixBateau(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            return Serialiser.DeserialiseBoolFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }
    }
}
