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
        public int Id { get; set; }
        public string Title { get; set; }
        public String Genre { get; set; }
        public bool IsSolo { get; set; }
        public bool IsMultiplayer { get; set; }
        public bool IsCoop { get; set; }
        public bool IsControllerCompatible { get; set; }
        public String CompatiblePlatforms { get; set; }
        public String StoreUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsFavorite { get; set; }
        public String Description { get; set; }
        public String ImagePath { get; set; }
        public DateTime LastPlayedDate { get; set; }
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
        }

        public Game(int id, string title, String genre, bool isSolo, bool isMultiplayer, bool isCoop, bool isControllerCompatible, String compatiblePlatorms, string storeUrl, DateTime releaseDate, bool isFavorite, String description, String imagePath, DateTime lastPlayedDate)
        {
            Id = id;
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
