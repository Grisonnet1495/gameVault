using System.Text.Json;
using System.Xml.Serialization;

namespace gameVaultClassLibrary
{
    public static class Backup
    {
        #region Methods
        public static void SetUpBackup(User user)
        {
            // Import user library if possible
            Library? library = ImportLibraryFromFile(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.xml"));

            if (library != null)
            {
                user.Library = library;
            }
        }

        public static void SaveDataToFile(User user)
        {
            // Save libraries config file and current user library
            ExportLibraryToFile(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.xml"), user.Library);
        }

        public static void ExportLibraryToFile(string filePath, Library library)
        {
            // Export library in XML
            var xmlSerializer = new XmlSerializer(typeof(Library));

            using (var writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, library);
            }
        }

        public static Library? ImportLibraryFromFile(string filePath)
        {
            // Import library in XML
            try
            {
                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(Library));
                        return xmlSerializer.Deserialize(reader) as Library;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static void ExportGameToFile(string filePath, Game game)
        {
            // Export game in XML
            XmlSerializer serializer = new XmlSerializer(typeof(Game));

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, game);
            }
        }


        public static Game? ImportGameFromFile(string filePath)
        {
            // Import a game in XML
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Game));

                    using (FileStream stream = new FileStream(filePath, FileMode.Open))
                    {
                        return (Game)serializer.Deserialize(stream);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static void ChangeUserPseudo(string oldPseudo, string newPseudo)
        {
            // Change filename of the user library file
            string oldLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{oldPseudo}_library.xml");
            string newLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{newPseudo}_library.xml");

            if (File.Exists(oldLibraryFilePath))
            {
                File.Move(oldLibraryFilePath, newLibraryFilePath);
            }
            else
            {
                throw new Exception("Cannot change the user library file");
            }
        }

        public static void ChangeAppDataFolder(string newAppDataPath)
        {
            string oldAppDataPath = Config.LoadSetting(Config.appDataKey);

            try
            {
                // Copy old directory to the new directory
                CopyDirectory(oldAppDataPath, newAppDataPath, overwrite: true);
                // Delete old directory
                Directory.Delete(oldAppDataPath, recursive: true);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }

            Config.SaveSetting(Config.appDataKey, newAppDataPath);
        }

        private static void CopyDirectory(string sourceDir, string destinationDir, bool overwrite)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string filename = Path.GetFileName(filePath);
                string destFile = Path.Combine(destinationDir, filename);
                File.Copy(filePath, destFile, overwrite);
            }

            foreach (string dirPath in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(dirPath);
                string destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectory(dirPath, destSubDir, overwrite);
            }
        }

        public static void DeleteUserData(User user)
        {
            // Delete all images from the user
            foreach (Game game in user.Library.GameList)
            {
                if (game.ImageName != null)
                {
                    string imagePath = System.IO.Path.Combine(System.IO.Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.imagesFolderKey)), game.ImageName);

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }

            // Delete the library file of the user
            File.Delete(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.xml"));
        } 
        #endregion
    }
}
