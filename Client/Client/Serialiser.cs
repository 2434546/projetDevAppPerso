using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Serialiser
    {
        /// <summary>
        /// Serialise la partie de TicTacToe en JSON
        /// </summary>
        /// <param name="game">La partie de jeu</param>
        /// <returns>Le Json de la partie</returns>
        public static string SerialiseBoardToJson(string game)
        {
            string json = JsonConvert.SerializeObject(game);
            return json;
        }
        /// <summary>
        /// Serialise la partie de TicTacToe en JSON avec des paramètres
        /// </summary>
        /// <param name="board">Le plateau de jeu</param>
        /// <param name="currentPlayer">Le joueur actuel</param>
        /// <param name="moves">Le nombre de mouvement de la partie</param>
        /// <param name="winner">Le gagnant de la partie</param>
        /// <returns>Le Json de la partie</returns>
        public static string SerialiseBoardToJson(char[] board, char currentPlayer, int moves, char winner)
        {
            //Remplacer game par le bord ou autre u jeu
            string game = "";
            string json = JsonConvert.SerializeObject(game);
            return json;
        }
        /// <summary>
        /// Deserialise la partie de TicTacToe du JSON et retourne la partie en objet TicTacToe
        /// </summary>
        /// <param name="json">Le Json de la partie</param>
        /// <returns>L'objet TicTacToe de la partie</returns>
        public static string DeserialiseBoardFromJson(string json)
        {

            //Remplacer game par le bord ou autre u jeu
            string game = "";
            return game;
        }
    }
}
