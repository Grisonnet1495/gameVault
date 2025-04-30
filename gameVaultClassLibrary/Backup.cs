using System.Text.Json;

namespace gameVaultClassLibrary
{
    public class Backup
    {
        private string usersLibrariesFilename = "usersLibraries.json";
        private string userLibraryFilename;
        private Dictionary<string, string> usersData;

        public Backup(User user)
        {
            // Create users file if it doesn't exist
            if (!File.Exists(usersLibrariesFilename))
            {
                File.WriteAllText(usersLibrariesFilename, "{}"); 
            }

            loadUsersLibrairiesFile(user);
        }

        private void loadUsersLibrairiesFile(User user)
        {
            if (!File.Exists(usersLibrariesFilename))
                return;

            // Load users file
            var jsonContent = File.ReadAllText(usersLibrariesFilename);

            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                // Retrieve all users data
                usersData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
            }
            else
            {
                usersData = new Dictionary<string, string>();
            }

            // Retrieve current user data
            if (usersData.ContainsKey(user.Pseudo))
            {
                userLibraryFilename = usersData[user.Pseudo];

                user.Library = importBackup(userLibraryFilename);
            }
        }

        public void saveDataToFile(User user)
        {
            userLibraryFilename = $"{user.Pseudo}_library.json";

            usersData[user.Pseudo] = userLibraryFilename;

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(usersLibrariesFilename, JsonSerializer.Serialize(usersData, jsonOptions));

            saveLibraryToFile(user.Library);
        }

        private void saveLibraryToFile(Library library)
        {
            var jsonOptionsLibrary = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringLibrary = JsonSerializer.Serialize(library, jsonOptionsLibrary);

            File.WriteAllText(userLibraryFilename, jsonStringLibrary);
        }

        private Library importBackup(String filename)
        {
            if (File.Exists(filename))
            {
                var libraryContent = File.ReadAllText(filename);

                if (!string.IsNullOrWhiteSpace(libraryContent))
                {
                    // Insert data in current user
                    return JsonSerializer.Deserialize<Library>(libraryContent);
                }
            }

            return null;
        }
    }
}
