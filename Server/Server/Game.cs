﻿using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    public class Game
    {
        private int gridSize;
        Tableau tableau;

        public Game()
        {
            this.gridSize = DemanderTailleGrille();
            this.tableau = new Tableau(gridSize);
        }

        public void StartGame(Socket socket)
        {
            string message = $"J'héberge un jeu avec une grille de taille {gridSize}x{gridSize}. Voulez-vous jouer ? (o/n)";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            socket.Send(messageBytes);

            byte[] buffer = new byte[1024];
            int bytesRec = socket.Receive(buffer);
            string reponse = Encoding.ASCII.GetString(buffer, 0, bytesRec).ToLower();

            if (reponse == "o")
            {
                Console.WriteLine("Le client a accepté de jouer.");
                Play(socket);
            }
            else
            {
                Console.WriteLine("Le client a refusé de jouer.");
                socket.Close();
            }
        }

        private void Play(Socket socket)
        {
            ChoisirBateau(socket);

            string win = "notWin";

            while (true)
            {
                string status;
                if (win == "notWin")
                    status = AdversaireJouer(socket);
                else
                    break;

                if (status == "continu")
                    win = JouerTour(socket);
                else
                    break;

            }

            RestartGame(socket);
        }

        public void RestartGame(Socket socket)
        {
            Console.WriteLine("Votre adversaire décide s'il veut refaire une partie ...");

            Tir? tir = tableau.RecevoirTir(socket);

            if(tir != null)
                if (tir.status == "newGame")
                {
                    tableau.ClearTableau();
                    StartGame(socket);
                }
        }

        public void ChoisirBateau(Socket socket)
        {
            //Attend que le serveur aille choisi
            Console.WriteLine("Votre adversaire choisi l'emplacement du bateau");
            bool bateauChoisiClient = RecevoirChoixBateau(socket);

            bool bateauChoisi = false;

            if (bateauChoisiClient)
            {
                AfficherJeux();

                //Choisi sont bateau
                bateauChoisi = tableau.ChoixBateau();
            }

            AfficherJeux();


            //Fait choisir le serveur
            EnvoyerChoixBateau(bateauChoisi, socket);
        }

        public string JouerTour(Socket socket)
        {
            Tir? tir = tableau.ChoixTir();
            tir.status = "toCheck";

            tableau.EnvoyerTir(tir, socket);

            tir = tableau.RecevoirTir(socket);

            if(tir != null)
            {
                if (tir.status == "win")
                {
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
            if (tir != null)
                tableau.EnvoyerTir(tir, socket);

            return "notWin";
        }

        public string AdversaireJouer(Socket socket)
        {
            Tir? tir = tableau.RecevoirTir(socket);
            if (tir != null)
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
                        if (tir != null)
                        {
                            if (tir.status == "changeTour")
                                nextTour = true;
                            if (tir.status == "newGame" || tir.status == "stop")
                                return "";
                        }
                    }

                }
            }
                return "continu";           
        }

        public void AfficherJeux()
        {
            Console.Clear();
            AfficherLegende();
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

        public int DemanderTailleGrille()
        {
            int taille;

            do
            {
                Console.WriteLine("Veuillez entrer la taille de la grille (entre 4 et 9) : ");
                taille = Convert.ToInt32(Console.ReadLine());
            } while (taille < 4 || taille > 9);

            return taille;
        }
    }
}
