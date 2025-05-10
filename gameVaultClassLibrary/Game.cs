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
        public string? ImageName { get; set; }
        public DateTime LastPlayedDate { get; set; }
        public string? GamePath { get; set; }
        public TimeSpan TimePlayed { get; set; }
        public double NbTimePlayed { get; set; }
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
            ImageName = null;
            LastPlayedDate = DateTime.Now;
            GamePath = null;
            TimePlayed = TimeSpan.Zero;
            NbTimePlayed = 0;
        }

        public Game(string title, string genre, bool isSolo, bool isMultiplayer, bool isCoop, bool isControllerCompatible, string compatiblePlatorms, string storeUrl, DateTime releaseDate, bool isFavorite, string description, string? imageName, DateTime lastPlayedDate, string? gamePath, TimeSpan timePlayed, double nbTimePlayed)
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
            ImageName = imageName;
            LastPlayedDate = lastPlayedDate;
            GamePath = gamePath;
            TimePlayed = timePlayed;
            NbTimePlayed = nbTimePlayed;
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
                $"{Description}" +
                $"{ImageName}" +
                $"{LastPlayedDate.ToString()}" +
                $"{GamePath}" +
                $"{TimePlayed.ToString()}" +
                $"{NbTimePlayed}" +
                "}";
        }
        #endregion
    }
}
