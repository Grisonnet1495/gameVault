using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    internal class Game
    {
        #region Properties
        public int Id { get; set; }
        public string GameName { get; set; }
        public List<string> GameGenre { get; set; }
        public List<string> GamePlatorm { get; set; }
        public List<string> GameMode { get; set; }
        public bool CompatibleWithController { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime ReleaseDate { get; set; }
        #endregion

        #region Constructor

        public Game()
        {
            Id = 0;
            GameName = "";
            GameGenre = new List<string>();
            GamePlatorm = new List<string>();
            GameMode = new List<string>();
            CompatibleWithController = false;
            IsFavorite = false;
            ReleaseDate = new DateTime();
        }

        public Game(int id, string gameName, List<string> gameGenre, List<string> gamePlatorm, List<string> gameMode, bool compatibleWithController, bool isFavorite, DateTime releaseDate)
        {
            Id = id;
            GameName = gameName;
            GameGenre = gameGenre;
            GamePlatorm = gamePlatorm;
            GameMode = gameMode;
            CompatibleWithController = compatibleWithController;
            IsFavorite = isFavorite;
            ReleaseDate = releaseDate;
        }
        #endregion

        #region Methods
        public void SetFavorite()
        {
            IsFavorite = !IsFavorite;
        }

        public override string ToString()
        {
            return $"Game: {Id} - {GameName} - {ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
        }
        #endregion

    }
}
