using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            tableau.ChoixBateau();

            Tir tir = tableau.ChoixTir();

            tableau.VerificationTir(tir);
        }
    }
}
