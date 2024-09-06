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
        Tableau tableau;

        public Game()
        {
            this.tableau = new Tableau();
        }

        public void StartGame(Socket socket)
        {
            //Attend que le serveur aille choisi
            /*bool bateauChoisiClient = RecevoirChoixBateau(socket);

            bool bateauChoisi = false;

            if (bateauChoisiClient)
            {
                //Choisi sont bateau
                bateauChoisi = tableau.ChoixBateau();
            }
           

            //Fait choisir le serveur
            EnvoyerChoixBateau(bateauChoisi, socket);*/

            

            while (tableau.gagnant == "")
            {
                Tir tir = tableau.RecevoirTir(socket);

                if (tir.status == "toCheck")
                {
                    tir = tableau.VerificationTir(tir);
                    tableau.EnvoyerTir(tir, socket);
                }

                bool nextTour = false;
                while (nextTour != true)
                {
                    tir = tableau.RecevoirTir(socket);
                    if (tir.status == "changeTour")
                        nextTour = true;
                }

                tir = tableau.ChoixTir();
                tir.status = "toCheck";

                tableau.EnvoyerTir(tir, socket);

                tir = tableau.RecevoirTir(socket);

                if(tir.status == "check")
                {
                    tableau.AjouterTir(tir);
                    tir.status = "changeTour";
                }
                        

                tableau.EnvoyerTir(tir, socket);
            }

            



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
