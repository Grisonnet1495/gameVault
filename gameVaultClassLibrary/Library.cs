using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    internal class Library
    {
        #region Properties
        public string LibraryName { get; set; }
        public List<Game> Games { get; private set; }
        #endregion

        #region constructor
        public Library()
        {
            LibraryName = "";
            Games = new List<Game>();
        }

        public Library(string libraryName)
        {
            LibraryName = libraryName;
            Games = new List<Game>();
        }
        #endregion

        #region methods
        public bool AddGame(Game game)
        {
            if(!Games.Any(g => g.GameName.Equals(game.GameName, StringComparison.OrdinalIgnoreCase)))//check if the game is already in the library with a case insensitive comparison
            {
                Games.Add(game);
                return true;
            }
            Console.WriteLine("Game already in the library");
            return false;
        }

        public bool RemoveGame(Game game)
        {
            if(Games.FirstOrDefault(g => g.GameName.Equals(game.GameName, StringComparison.OrdinalIgnoreCase)) != null)//check if the game is in the library with a case insensitive comparison
            {
                Games.Remove(game);
                return true;
            }
            Console.WriteLine("Game not in the library");
            return false;
        }

        public Game SearchGame(string gameName)
        {
            Game game = Games.FirstOrDefault(SearchGame => SearchGame.GameName.Equals(gameName, StringComparison.OrdinalIgnoreCase));//search for the game with a case insensitive comparison

            if(game != null)
            {
                return game;
            }
            return null;
        }

        public void SortGame()
        {
            Games = Games.OrderBy(g => g.GameName).ToList();//sort the games by name
        }
        #endregion
    }
}
