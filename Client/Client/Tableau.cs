using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Tableau
    {
        public string[] tableauJoueur { get; set; }
        public string[] tableauAdversaire { get; set; }

        public Tableau()
        {
            this.tableauJoueur = tableauJoueur;
            this.tableauAdversaire = tableauAdversaire;
        }

        public string VerificationTir()
        {
            return "";
        }

        public string AffichageTableau()
        {
            return "";
        }

        public string EnvoyerTir()
        {
            return "";
        }

        public string ChoixTir()
        {
            return "";
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
