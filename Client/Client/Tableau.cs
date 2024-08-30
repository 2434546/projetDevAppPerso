using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Tableau
    {
        public string[] tableauJoueur { get; set; }
        public string[] tableauAdversaire { get; set; }
        int size;

        public Tableau()
        {
            size = 4;
            this.tableauJoueur = new string[size * size];
            this.tableauAdversaire = new string[size * size];
        }

        public string VerificationTir()
        {
            return "";
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

        public string EnvoyerTir()
        {
            return "";
        }

        public string ChoixTir()
        {
            return "";
        }

        public string ChoixBateau()
        {
            return "";
        }

        public string EnvoieConfirmation()
        {
            return "";
        }
    }
}
