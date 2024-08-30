using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Serialiser
    {
        /// <summary>
        /// Serialise la partie de TicTacToe en JSON
        /// </summary>
        /// <param name="game">La partie de jeu</param>
        /// <returns>Le Json de la partie</returns>
        public static string SerialiseTirToJson(Tir tir)
        {
            string json = JsonConvert.SerializeObject(tir);
            return json;
        }

        /// <summary>
        /// Deserialise la partie de TicTacToe du JSON et retourne la partie en objet TicTacToe
        /// </summary>
        /// <param name="json">Le Json de la partie</param>
        /// <returns>L'objet TicTacToe de la partie</returns>
        public static Tir? DeserialiseTirFromJson(string json)
        {
            Tir? tir = JsonConvert.DeserializeObject<Tir>(json);
            return tir;
        }

        public static string SerialiseBoolToJson(bool boolean)
        {
            string json = JsonConvert.SerializeObject(boolean);
            return json;
        }

        public static bool DeserialiseBoolFromJson(string json)
        {
            bool boolean = JsonConvert.DeserializeObject<bool>(json);
            return boolean;
        }
    }
}
