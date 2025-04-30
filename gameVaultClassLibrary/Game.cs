using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    public class Game
    {
        #region Properties
        public int Id { get; set; } // Id is correctly settled when the game is added to the library
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool IsSolo { get; set; }
        public bool IsMultiplayer { get; set; }
        public bool IsCoop { get; set; }
        public bool IsControllerCompatible { get; set; }
        public string CompatiblePlatforms { get; set; }
        public string StoreUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsFavorite { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime LastPlayedDate { get; set; }
        public string GamePath { get; set; }
        public TimeSpan TimePlayed { get; set; }
        #endregion

        #region Constructor
        public Game()
        {
            Id = 0;
            Title = "Unknown title";
            Genre = "No info";
            IsSolo = false;
            IsMultiplayer = false;
            IsCoop = false;
            IsControllerCompatible = false;
            CompatiblePlatforms = "No info";
            StoreUrl = "No store";
            ReleaseDate = new DateTime();
            IsFavorite = false;
            Description = "No description";
            ImagePath = "Ressources/Images/default_game_image.png";
            LastPlayedDate = DateTime.Now;
            GamePath = string.Empty;
            TimePlayed = TimeSpan.Zero;
        }

        public Game(string title, string genre, bool isSolo, bool isMultiplayer, bool isCoop, bool isControllerCompatible, string compatiblePlatorms, string storeUrl, DateTime releaseDate, bool isFavorite, string description, string imagePath, DateTime lastPlayedDate, string gamePath)
        {
            Id = 0;
            Title = title;
            Genre = genre;
            IsSolo = isSolo;
            IsMultiplayer = isMultiplayer;
            IsCoop = isCoop;
            IsControllerCompatible = isControllerCompatible;
            CompatiblePlatforms = compatiblePlatorms;
            StoreUrl = storeUrl;
            ReleaseDate = releaseDate;
            IsFavorite = isFavorite;
            Description = description;
            ImagePath = imagePath;
            LastPlayedDate = lastPlayedDate;
            GamePath = gamePath;
            TimePlayed = TimeSpan.Zero;
        }
        #endregion

        #region Methods
        public void toggleFavorite()
        {
            IsFavorite = !IsFavorite;
        }

        public override string ToString()
        {
            return $"Game: " +
                "{" +
                $"{Id}; " +
                $"{Title}; " +
                $"{Genre}; " +
                $"{IsSolo}; " +
                $"{IsMultiplayer}; " +
                $"{IsCoop}; " +
                $"{IsControllerCompatible}; " +
                $"{CompatiblePlatforms}; " +
                $"{StoreUrl};" +
                $"{ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}; " +
                $"{IsFavorite}" +
                "}";
        }
        #endregion
    }
}
