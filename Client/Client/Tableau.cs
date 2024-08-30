using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            int size = 4;
            this.tableauJoueur = new string[size];
            this.tableauAdversaire = new string[size];
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

        public string AffichageTableau()
        {
            return "";
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
