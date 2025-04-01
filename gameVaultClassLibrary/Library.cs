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
        public List<Game> Games { get; set; }
        #endregion

        #region constructor
        public Library(string libraryName = "My game Library")
        {
            LibraryName = libraryName;
            Games = new List<Game>();
        }
        #endregion

        #region methods
        public void AddGame(Game game)
        {
            if (!Games.Contains(game))
            {
                Games.Add(game);
                return;
            }
            Console.WriteLine("Game already in the library");
        }

        public void RemoveGame(Game game)
        {
            if (Games.Contains(game))
            {
                Games.Remove(game);
                return;
            }
            Console.WriteLine("Game not in the library");
        }
        #endregion
    }
}
