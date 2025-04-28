using System.Text.Json;

namespace gameVaultClassLibrary
{
    public class Backup
    {
        private string fileNameUser = "usersFile.json";
        private string fileNameUserLibrary;
        private Dictionary<string, string> usersData = new();

        public Backup(User user)
        {
            if (!File.Exists(fileNameUser))
            {
                File.WriteAllText(fileNameUser, "{}"); 
            }
            loadDataFromFile(user);
        }

        private void loadDataFromFile(User user)
        {
            if (!File.Exists(fileNameUser))
                return;

            var jsonContent = File.ReadAllText(fileNameUser);
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                usersData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
            }

            if (usersData.ContainsKey(user.Pseudo))
            {
                fileNameUserLibrary = usersData[user.Pseudo];

                if (File.Exists(fileNameUserLibrary))
                {
                    var libraryContent = File.ReadAllText(fileNameUserLibrary);
                    if (!string.IsNullOrWhiteSpace(libraryContent))
                    {
                        user.UserLibrary = JsonSerializer.Deserialize<Library>(libraryContent);
                    }
                }
            }
        }

        public void saveDataToFile(User user)
        {
            fileNameUserLibrary = $"{user.Pseudo}_{user.UserLibrary.LibraryName}.json";

            usersData[user.Pseudo] = fileNameUserLibrary;

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(fileNameUser, JsonSerializer.Serialize(usersData, jsonOptions));

            saveGameInFileLibrary(user.UserLibrary);
        }

        private void saveGameInFileLibrary(Library library)
        {
            var jsonOptionsLibrary = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringLibrary = JsonSerializer.Serialize(library, jsonOptionsLibrary);

            File.WriteAllText(fileNameUserLibrary, jsonStringLibrary);
        }
    }
}
