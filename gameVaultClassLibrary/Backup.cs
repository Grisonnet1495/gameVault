using System.Text.Json;
using System.Xml.Serialization;

namespace gameVaultClassLibrary
{
    public class Backup
    {
        #region Properties
        //private string allLibrariesFilename; // Filename of the libraries config file
        //private string userLibraryFilename; // Library Filename of the current user
        //private Dictionary<string, string> allLibrariesData; // Contains data of the libraries config file
        //private User currentUser;
        #endregion

        //#region Constructor
        //public Backup(User user)
        //{
        //    Config.SetUpConfig();

        //    currentUser = user;

        //    SetUpBackup();
        //}
        //#endregion

        //private void SetUpBackup()
        //{
        //    allLibrariesFilename = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.librariesConfigKey));

        //    // Create libraries config file if it doesn't exist
        //    if (!File.Exists(allLibrariesFilename))
        //    {
        //        File.WriteAllText(allLibrariesFilename, "{}");
        //    }

        //    // Load the libraries config file
        //    var jsonContent = File.ReadAllText(allLibrariesFilename);

        //    if (!string.IsNullOrWhiteSpace(jsonContent))
        //    {
        //        // Retrieve libraries config data
        //        allLibrariesData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
        //    }
        //    else
        //    {
        //        allLibrariesData = new Dictionary<string, string>();
        //    }

        //    // Retrieve or create current user libray filename
        //    if (allLibrariesData.ContainsKey(currentUser.Pseudo))
        //    {
        //        userLibraryFilename = allLibrariesData[currentUser.Pseudo];
        //    }
        //    else
        //    {
        //        userLibraryFilename = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{currentUser.Pseudo}_library.json");
        //    }

        //    // Import user library if possible
        //    Library library = ImportLibraryFromFile(userLibraryFilename);

        //    if (library != null)
        //    {
        //        currentUser.Library = library;
        //    }
        //}

        public static void SetUpBackup(User user)
        {
            // Import user library if possible
            Library library = ImportLibraryFromFile(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.json"));

            if (library != null)
            {
                user.Library = library;
            }
        }

        // Save libraries config file and current user library
        public static void SaveDataToFile(User user)
        {
            // Save the library of the current user
            ExportLibraryToFile(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.json"), user.Library);
        }

        // Export library in JSON
        public static void ExportLibraryToFile(string filePath, Library library)
        {
            var jsonOptionsLibrary = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringLibrary = JsonSerializer.Serialize(library, jsonOptionsLibrary);

            File.WriteAllText(filePath, jsonStringLibrary);
        }

        // Import library in JSON
        public static Library ImportLibraryFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileContent = File.ReadAllText(filePath);

                if (!string.IsNullOrWhiteSpace(fileContent))
                {
                    return JsonSerializer.Deserialize<Library>(fileContent);
                }
            }

            return null;
        }

        // Export game in XML
        public static void ExportGameToFile(string filePath, Game game)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Game));

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, game);
            }
        }


        // Import a game in XML
        public static Game ImportGameFromFile(string filePath)
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

        // Note : To verify
        // Change an user pseudo
        public static void ChangeUserPseudo(string oldPseudo, string newPseudo)
        {
            // Change pseudo in passwords file
            //string passwordsFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.userConfigKey));
            //if (!File.Exists(passwordsFilePath))
            //{
            //    throw new Exception("Canot find the user passwords file");
            //}

            //var passwordsData = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(passwordsFilePath));
            //if (passwordsData != null && passwordsData.ContainsKey(oldPseudo))
            //{
            //    string password = passwordsData[oldPseudo];
            //    passwordsData.Remove(oldPseudo);
            //    passwordsData[newPseudo] = password;

            //    File.WriteAllText(passwordsFilePath, JsonSerializer.Serialize(passwordsData, new JsonSerializerOptions { WriteIndented = true }));
            //}
            //else
            //{
            //    throw new Exception("Invalid data in the user passwords file");
            //}

            // Change pseudo in libraries config file
            //string allLibrariesFilename = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.librariesConfigKey));
            //if (!File.Exists(allLibrariesFilename))
            //{
            //    throw new Exception("Canot find the libraries config file");
            //}

            //var allLibrariesData = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(allLibrariesFilename));
            //if (allLibrariesData != null && allLibrariesData.ContainsKey(oldPseudo))
            //{
            //    string libraryFilePath = allLibrariesData[oldPseudo];
            //    allLibrariesData.Remove(oldPseudo);
            //    allLibrariesData[newPseudo] = libraryFilePath;

            //    File.WriteAllText(allLibrariesFilename, JsonSerializer.Serialize(allLibrariesData, new JsonSerializerOptions { WriteIndented = true }));
            //}
            //else
            //{
            //    throw new Exception("Invalid data in the libraries config file");
            //}

            // Change filename of the user library file
            string oldLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{oldPseudo}_library.json");
            string newLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{newPseudo}_library.json");

            if (File.Exists(oldLibraryFilePath))
            {
                File.Move(oldLibraryFilePath, newLibraryFilePath);
            }
            else
            {
                throw new Exception("Cannot change the user library file");
            }
        }

        // Change the app data folder
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

            //// Reload the backup
            //SetUpBackup();
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
            // Note : To continue
            // Note : Delete all images from the user

            File.Delete(Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.json"));
        }
    }
}
