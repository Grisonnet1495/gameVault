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
        #region Properties
        public Dictionary<string, string> Users { get; set; } 
        #endregion

        #region Contructor
        public Authenticator()
        {
            Config.SetUpConfig();

            string userFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.userConfigKey));

            // Create user file if it doesn't exist
            if (!File.Exists(userFilePath))
            {
                File.WriteAllText(userFilePath, "{}");
            }

            LoadUserFile();
        }
        #endregion

        #region Methods
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

        private string? GetPassword(string pseudo)
        {
            if (UserExists(pseudo))
            {
                return Users[pseudo];
            }

            return null;
        }

        public bool ChangePassword(string pseudo, string newPassword)
        {
            if (!Users.ContainsKey(pseudo))
                return false;

            Users[pseudo] = newPassword;

            SaveUserFile();

            return true;
        }

        public bool ChangeUserPseudo(string oldPseudo, string newPseudo)
        {
            if (UserExists(newPseudo))
            {
                return false;
            }

            string? password = GetPassword(oldPseudo);

            if (password == null)
            {
                return false;
            }

            RemoveUser(oldPseudo);
            AddUser(newPseudo, password);

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

        public void LoadUserFile()
        {
            string userFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.userConfigKey));

            if (!File.Exists(userFilePath))
                return;

            string json = File.ReadAllText(userFilePath);
            var users = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (users != null)
            {
                Users = users;
            }
            else
            {
                Users = new Dictionary<string, string>();
            }
        }

        public void SaveUserFile()
        {
            string userFilePath = Path.Combine(Config.LoadSetting(Config.appDataKey), Config.LoadSetting(Config.userConfigKey));

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(userFilePath, json);
        } 
        #endregion
    }
}
