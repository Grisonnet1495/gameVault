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
        public int currentImageNb { get; set; }
        public List<Game> GameList { get; set; }
        #endregion

        #region Constructor
        public Library()
        {
            LibraryName = "";
            currentImageNb = 0;
            GameList = new List<Game>();
        }

        public Library(string libraryName)
        {
            LibraryName = libraryName;
            currentImageNb = 0;
            GameList = new List<Game>();
        }
        #endregion

        #region Methods
        public bool AddGame(Game game)
        {
            // Generate a new id
            while (GameList.Any(g => g.Id == game.Id))
            {
                game.Id++;
            }

            GameList.Add(game);
            return true;
        }

        public bool RemoveGame(Game game)
        {
            if(GameList.FirstOrDefault(g => g.Id == game.Id) != null)//check if the game is in the library with a case insensitive comparison
            {
                GameList.Remove(game);
                return true;
            }

            Console.WriteLine("Game not in the library");
            return false;
        }

        public Game? FindGameByTitle(string gameName)
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

        public TimeSpan CalculateGameTime()
        {
            TimeSpan totalGameTime = TimeSpan.Zero;

            foreach (Game game in GameList)
            {
                totalGameTime += game.TimePlayed;
            }

            return totalGameTime;
        }
        #endregion
    }
}
