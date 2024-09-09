﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Tableau
    {
        public string[] tableauJoueur { get; set; }
        public string[] tableauAdversaire { get; set; }
        public string gagnant;
        int size;
        int caseBateauPlace1;
        int caseBateauPlace2;

        public Tableau()
        {
            size = 4;
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
            this.gagnant = "";
        }

        public Tir VerificationTir(Tir tir)
        {
            string emplacement = tableauJoueur[tir.coord - 1];
            if(emplacement == "BB")
            {
                tableauAdversaire[tir.coord - 1] = "BT";
                tir.hit = true;
            }
           
            tableauAdversaire[tir.coord - 1] = "XX";

            tir.status = "check";
            return tir;
        }

        public void AffichageTableauJoueur()
        {
            double gridDimensionJoueur = Math.Sqrt(tableauJoueur.Length);
            int index = 1;

            for (int i = 0; i < gridDimensionJoueur; i++)
            {
                for (int j = 0; j < gridDimensionJoueur; j++)
                {
                    if (tableauJoueur[(i * size) + j] != null)
                    {
                        Console.Write($" {tableauJoueur[(i * size) + j]} |");
                    }

                    else
                    {
                        if (index < 10)
                            Console.Write($" 0{index} |");

                        else
                            Console.Write($" {index} |");
                    }
                    index++;
                }

                if (i < gridDimensionJoueur - 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("--------------------");
                }

            }
        }

        public void AffichageTableauAdversaire()
        {
            double gridDimensionAdversaire = Math.Sqrt(tableauAdversaire.Length);
            int index = 1;

            for (int i = 0; i < gridDimensionAdversaire; i++)
            {
                for (int j = 0; j < gridDimensionAdversaire; j++)
                {

                        if(tableauAdversaire[(i * size) + j] != null)
                        {
                            Console.Write($" {tableauAdversaire[(i * size) + j]} |");
                        }

                        else
                        {
                            if(index < 10)
                            Console.Write($" 0{index} |");

                            else
                            Console.Write($" {index} |");
                        }
                        index++;
                        
                }
                if (i < gridDimensionAdversaire - 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("--------------------");
                }

            }
        }

        public void EnvoyerTir(Tir tir, Socket socket)
        {
            string JsonTir = Serialiser.SerialiseTirToJson(tir);
            byte[] bytes = Encoding.ASCII.GetBytes(JsonTir);
            socket.Send(bytes);
        }

        public Tir? RecevoirTir(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int bytesRecu = socket.Receive(bytes);
            return Serialiser.DeserialiseTirFromJson(Encoding.ASCII.GetString(bytes, 0, bytesRecu));
        }

        public Tir ChoixTir()
        {
            //TODO Faire une vérification pour empecher de tirer à la meme place

            int coordChoisi = 0;
            bool caseValide = false;
            
            do
            {
                Console.WriteLine("Veuillez choisir un une position ou tirer dans le tableau");
                coordChoisi = Convert.ToInt32(Console.ReadLine());

                if (coordChoisi >= 1 && coordChoisi <= size * size)
                {
                    if (tableauAdversaire[coordChoisi - 1] != "XX" && tableauAdversaire[coordChoisi - 1] != "BT")
                    {
                        caseValide = true;
                    }
                    else
                    {
                        Console.WriteLine("La case a déjà été choisie.");
                    }
                }
                else
                {
                    Console.WriteLine("Coord hors limites");
                }
            }
            while (!caseValide);

            Tir tir = new Tir(coordChoisi);
            return tir;
        }

        public bool ChoixBateau()
        {
            bool placementValide = false;
            int case1, case2;
            string[] coord;

            do
            {
                Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input) && input.Contains(","))
                {
                    coord = input.Split(",");

                    if (coord.Length == 2 && int.TryParse(coord[0], out case1) && int.TryParse(coord[1], out case2))
                    {
                        if (case1 >= 1 && case1 <= size * size && case2 >= 1 && case2 <= size * size)
                        {

                            if (Math.Abs(case1 - case2) == 1 || Math.Abs(case1 - case2) == size)
                            {
                                if (Math.Abs(case1 - case2) == 1 && (Math.Max(case1, case2) % size == 1))
                                {
                                    Console.WriteLine("Les cases sélectionné sont placées sur deux lignes différentes opposé");
                                }
                                else if (tableauJoueur[case1 - 1] == null && tableauJoueur[case2 - 1] == null)
                                {
                                    tableauJoueur[case1 - 1] = "BB";
                                    tableauJoueur[case2 - 1] = "BB";
                                    Console.WriteLine($"Le bateau a été placé aux coordonnées {case1} et {case2}");
                                    placementValide = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Les coordonnées ne sont pas alignées. Le bateau doit être placé horizontalement ou verticalement.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Coordonnées hors limites. Veuillez choisir des positions valides dans le tableau.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrée invalide. Veuillez entrer des nombres entiers.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer deux entiers séparés par une virgule.");
                }

            } while (!placementValide);

            return placementValide;
        }

        public string EnvoieConfirmation()
        {
            return "";
        }

        public bool VerifierGagnant()
        {
            if (tableauJoueur[caseBateauPlace1] == "BT" && tableauJoueur[caseBateauPlace2] == "BT")
            {
                return true;
            }

            else
                return false;
                
        }
    }
}
