using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    public class Library
    {
        #region Properties
        public string LibraryName { get; set; }
        public List<Game> GameList { get; set; }
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
            // If the game is in the library
            if (GameList.FirstOrDefault(g => g.Id == game.Id) != null)
            {
                if (game.ImageName != null)
                {
                    string imagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), game.ImageName);

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                GameList.Remove(game);
                return true;
            }

            return false;
        }

        public Game? FindGameByTitle(string gameName)
        {
            Game game = GameList.FirstOrDefault(SearchGame => SearchGame.Title.Equals(gameName, StringComparison.OrdinalIgnoreCase));

            if(game != null)
            {
                return game;
            }

            return null;
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

        public override string ToString()
        {
            return $"Library: " +
                "{" +
                $"{LibraryName}; " +
                $"{GameList.ToString}" + 
                "}";
        }
        #endregion
    }
}
