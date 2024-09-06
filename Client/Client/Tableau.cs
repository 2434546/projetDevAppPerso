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
            
            do
            {
                Console.WriteLine("Veuillez choisir un une position ou tirer dans le tableau");
                coordChoisi = Convert.ToInt32(Console.ReadLine());
            }
            while (coordChoisi > 17 && coordChoisi < 0);

            Tir tir = new Tir(coordChoisi);
            return tir;
        }

        public bool ChoixBateau()
        {

            //TODO Vérifier entré pas > 0 ou < que taille tableau Redemander question si bateau pas valide
            string input = "";
            string[] coord;

            do
            {
                Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
                input = Console.ReadLine();
                coord = input.Split(",");
                
                if (coord.Length == 2 && int.TryParse(coord[0], out int case1) && int.TryParse(coord[1], out int case2) &&
                    case1 >= 0 && case1 < size && case2 >= 0 && case2 < size)
                {
                    break;
                }

            } while(true);

            if (Math.Abs(case1 - case2) == 1 || Math.Abs(case1 - case2) == size)
            {
                //return true;

                if (tableauJoueur[case1 - 1] == null && tableauJoueur[case2 - 1] == null)
                {
                    tableauJoueur[case1 - 1] = "BB";
                    tableauJoueur[case2 - 1] = "BB";

                    Console.WriteLine($"Le bateau a été placé aux coordonnées {case1} et {case2}");
                    return true;
                }

                return true;
            }
            else
            {
                Console.WriteLine("Vous ne pouvez placer votre bateau que de façon vertical ou horizontal");
                return false;
            }
        
        }

        public string EnvoieConfirmation()
        {
            return "";
        }

        public bool VerifierGagnant()
        {
            return true;
        }
    }
}
