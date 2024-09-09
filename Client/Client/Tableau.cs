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
        private string[] tableauJoueur { get; set; }
        private string[] tableauAdversaire { get; set; }
        int size;
        int caseBateauPlace1;
        int caseBateauPlace2;

        public Tableau()
        {
            size = 4;
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
        }

        public Tir VerificationTir(Tir tir)
        {
            string emplacement = tableauJoueur[tir.coord - 1];
            if(emplacement == "BB")
            {
                tableauJoueur[tir.coord - 1] = "BT";
                tir.hit = true;
            }
            else
            {
                tableauJoueur[tir.coord - 1] = "XX";
            }

            tir.status = "check";
            return tir;
        }

        public void AjoutTir(Tir tir)
        {
            if (tir.hit)
            {
                tableauAdversaire[tir.coord - 1] = "BT";
            }
            else
            {
                tableauAdversaire[tir.coord - 1] = "XX";
            }

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
                Console.WriteLine("Veuillez choisir une position où tirer dans le tableau");
                coordChoisi = Convert.ToInt32(Console.ReadLine());
            }
            while (coordChoisi > 17 && coordChoisi < 0);

            Tir tir = new Tir(coordChoisi);
            return tir;
        }

        public bool ChoixBateau()
        {
            //TODO Faire une classe bateau ou on peut ajouter différentes dimension de bateau

            //TODO Vérifier entré pas > 0 ou < que taille tableau Redemander question si bateau pas valide

            Console.WriteLine("Entrez les coordonnées de votre bateau (ex: 4,5) : ");
            string input = Console.ReadLine();
            string[]? coord;

            coord = input.Split(",");

            if(coord != null)
                coord = input.Split(".");

            if (!int.TryParse(coord[0], out caseBateauPlace1) || !int.TryParse(coord[1], out caseBateauPlace2))
            {
                Console.WriteLine("IL FAUT SAISIR UN ENTIER");
                return false;
            }

            if (Math.Abs(caseBateauPlace1 - caseBateauPlace2) == 1 || Math.Abs(caseBateauPlace1 - caseBateauPlace2) == size)
            {
                //return true;

                if (tableauJoueur[caseBateauPlace1 - 1] == null && tableauJoueur[caseBateauPlace2 - 1] == null)
                {
                    tableauJoueur[caseBateauPlace1 - 1] = "BB";
                    tableauJoueur[caseBateauPlace2 - 1] = "BB";

                    Console.WriteLine($"Le bateau a été placé aux coordonnées {caseBateauPlace1} et {caseBateauPlace2}");
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

        public bool VerifierGagnant()
        {

            //TODO s'organiser pour que le code soit facilement modifiable si des s'ajoute au jeu ou si les dimensions changent
            if (tableauJoueur[caseBateauPlace1 - 1] == "BT" && tableauJoueur[caseBateauPlace2 - 1] == "BT")
            {
                return true;
            }

            else
                return false;
                
        }

        public void ClearTableau()
        {
            tableauAdversaire = new string[size * size];
            tableauJoueur = new string[size * size];
        }

        

    }
}
