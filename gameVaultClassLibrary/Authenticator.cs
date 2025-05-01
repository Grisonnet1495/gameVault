using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace gameVaultClassLibrary
{
    public class Authenticator
    {
        private string userFilePath; // User password file path
        public Dictionary<string, string> Users { get; set; }

        public Authenticator()
        {
            Config.SetUpConfig();

            userFilePath = Config.LoadSetting(Config.userConfigKey);

            // Create user file if it doesn't exist
            if (!File.Exists(userFilePath))
            {
                File.WriteAllText(userFilePath, "{}");
            }

            Users = new Dictionary<string, string>();

            LoadUserFile();
        }

        public bool AddUser(string pseudo, string password)
        {
            if (Users.ContainsKey(pseudo))
            {
                return false;
            }

            Users[pseudo] = password;

            SaveUserFile();

            return true;
        }

        public bool RemoveUser(string pseudo)
        {
            if (!Users.Remove(pseudo))
            {
                return false;
            }

            SaveUserFile();

            return true;
        }

        public bool ChangePassword(string pseudo, string newPassword)
        {
            if (!Users.ContainsKey(pseudo))
                return false;

            Users[pseudo] = newPassword;

            SaveUserFile();

            return true;
        }

        public bool UserExists(string pseudo)
        {
            return Users.ContainsKey(pseudo);
        }

        public bool AuthenticateUser(string pseudo, string password)
        {
            if (GetPassword(pseudo) == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string? GetPassword(string pseudo)
        {
            if (Users.ContainsKey(pseudo))
            {
                return Users[pseudo];
            }

            return null;
        }

        public void LoadUserFile()
        {
            if (!File.Exists(userFilePath))
                return;

            string json = File.ReadAllText(userFilePath);
            var users = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (users != null)
            {
                Users = users;
            }
        }

        public void SaveUserFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(userFilePath, json);
        }
    }
}
