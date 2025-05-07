using System.Text.Json;
using System.Xml.Serialization;

namespace gameVaultClassLibrary
{
    public class Backup
    {
        private string allLibrariesFilename; // Filename of the libraries config file
        private string userLibraryFilename; // Library Filename of the current user
        private Dictionary<string, string> allLibrariesData; // Contains data of the libraries config file
        private Config config;

        public Backup(User user)
        {
            Config.SetUpConfig();

            allLibrariesFilename = Config.LoadSetting(Config.librariesConfigKey);

            // Create libraries config file if it doesn't exist
            if (!File.Exists(allLibrariesFilename))
            {
                File.WriteAllText(allLibrariesFilename, "{}");
            }

            // Load the libraries config file
            LoadAllLibrariesFile(user);

            // Import user library if possible
            Library library = ImportLibraryFromFile(userLibraryFilename);

            if (library != null)
            {
                user.Library = library;
            }
        }

        // Load all libraries config file data and set the current user library filename
        private void LoadAllLibrariesFile(User user)
        {
            if (!File.Exists(allLibrariesFilename))
                return;

            // Load users file
            var jsonContent = File.ReadAllText(allLibrariesFilename);

            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                // Retrieve all users data
                allLibrariesData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
            }
            else
            {
                allLibrariesData = new Dictionary<string, string>();
            }

            // Retrieve or create current user libray filename
            if (allLibrariesData.ContainsKey(user.Pseudo))
            {
                userLibraryFilename = allLibrariesData[user.Pseudo];
            }
            else
            {
                userLibraryFilename = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{user.Pseudo}_library.json");
            }
        }

        // Save libraries config file and current user library
        public void SaveDataToFile(User user)
        {
            allLibrariesData[user.Pseudo] = userLibraryFilename;

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(allLibrariesFilename, JsonSerializer.Serialize(allLibrariesData, jsonOptions));

            ExportLibraryToFile(userLibraryFilename, user.Library);
        }

        // Save current user library in JSON
        public static void ExportLibraryToFile(string filePath, Library library)
        {
            var jsonOptionsLibrary = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringLibrary = JsonSerializer.Serialize(library, jsonOptionsLibrary);

            File.WriteAllText(filePath, jsonStringLibrary);
        }

        // Import library to current user in JSON
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
        public static bool ChangeUserPseudo(string oldPseudo, string newPseudo)
        {
            Config.SaveSetting(oldPseudo, newPseudo);

            string allLibrariesFilename = Config.LoadSetting(Config.librariesConfigKey);
            if (!File.Exists(allLibrariesFilename))
                return false;

            var allLibrariesData = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(allLibrariesFilename));
            if (allLibrariesData != null && allLibrariesData.ContainsKey(oldPseudo))
            {
                string libraryFilePath = allLibrariesData[oldPseudo];
                allLibrariesData.Remove(oldPseudo);
                allLibrariesData[newPseudo] = libraryFilePath;

                File.WriteAllText(allLibrariesFilename, JsonSerializer.Serialize(allLibrariesData, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                return false;
            }

            string passwordsFilePath = Config.LoadSetting(Config.userConfigKey);
            if (!File.Exists(passwordsFilePath)) return false;

            var passwordsData = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(passwordsFilePath));
            if (passwordsData != null && passwordsData.ContainsKey(oldPseudo))
            {
                string password = passwordsData[oldPseudo];
                passwordsData.Remove(oldPseudo);
                passwordsData[newPseudo] = password;

                File.WriteAllText(passwordsFilePath, JsonSerializer.Serialize(passwordsData, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                return false; 
            }

            string oldLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{oldPseudo}_library.json");
            string newLibraryFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), $"{newPseudo}_library.json");

            if (File.Exists(oldLibraryFilePath))
            {
                File.Move(oldLibraryFilePath, newLibraryFilePath);
            }
            else
            {
                return false;
            }

            return true;

        }

    }
}
