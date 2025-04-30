using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    public class Library
    {
        #region Properties
        public string LibraryName { get; set; }
        public List<Game> GameList { get; private set; }
        #endregion

        #region Constructor
        public Library()
        {
            LibraryName = "";
            GameList = new List<Game>();
        }

        public Library(string libraryName)
        {
            LibraryName = libraryName;
            GameList = new List<Game>();
        }
        #endregion

        #region Methods
        public bool AddGame(Game game)
        {
            if(!GameList.Any(g => g.Title.Equals(game.Title, StringComparison.OrdinalIgnoreCase)))//check if the game is already in the library with a case insensitive comparison
            {
                GameList.Add(game);
                return true;
            }
            Console.WriteLine("Game already in the library");
            return false;
        }

        public bool RemoveGame(Game game)
        {
            if(GameList.FirstOrDefault(g => g.Title.Equals(game.Title, StringComparison.OrdinalIgnoreCase)) != null)//check if the game is in the library with a case insensitive comparison
            {
                GameList.Remove(game);
                return true;
            }
            Console.WriteLine("Game not in the library");
            return false;
        }

        public Game SearchGame(string gameName)
        {
            Game game = GameList.FirstOrDefault(SearchGame => SearchGame.Title.Equals(gameName, StringComparison.OrdinalIgnoreCase));//search for the game with a case insensitive comparison

            if(game != null)
            {
                return game;
            }

            return null;
        }

        public void SortGame()
        {
            GameList = GameList.OrderBy(g => g.Title).ToList();//sort the games by name
        }
        #endregion
    }
}
