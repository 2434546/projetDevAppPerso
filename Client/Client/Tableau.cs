using System;
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

        public Tableau()
        {
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
            this.gagnant = "";
        }

        public Tir VerificationTir(Tir tir)
        {
            string emplacement = tableauJoueur[tir.coord];
            if(emplacement == "B")
            {
                tir.hit = true;
            }

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
                        Console.Write($"{tableauJoueur[(i * size) + j]}|");
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
                            Console.Write($"{tableauAdversaire[(i * size) + j]}|");
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
            int coordChoisi = 0;
            Tir tir = new Tir(coordChoisi);

            do
            {
                Console.WriteLine("Veuillez choisir un une position ou tirer dans le tableau");
                coordChoisi = Convert.ToInt32(Console.ReadLine());
            }
            while (coordChoisi < 17 && coordChoisi > 0);
            
            return tir;
        }

        public void AjouterTir(Tir tir)
        {

        }

        public bool ChoixBateau(bool joueur)
        {
            Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
            string input = Console.ReadLine();

            string[] coord = input.Split(",");

            if (!int.TryParse(coord[0], out int case1) || !int.TryParse(coord[1], out int case2))
            {
                Console.WriteLine("IL FAUT UN INT");
                return false;
            }

            if (Math.Abs(case1 - case2) == 1)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Vous ne pouvez placer votre bateau que de façon vertical ou horizontal");
                return false;
            }

            string[] tableau = joueur ? tableauJoueur : tableauAdversaire;

            if (tableau[case1] == "" && tableau[case2] == "")
            {
                tableau[case1] = "B";
                tableau[case2] = "B";

                Console.WriteLine($"Le bateau a été placé aux coordonnées {case1} et {case2}");
                return true;
            }

            return true;
        }

        public string EnvoieConfirmation()
        {
            return "";
        }
    }
}
