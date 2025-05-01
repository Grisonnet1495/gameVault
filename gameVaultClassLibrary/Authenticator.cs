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
        private string userFilePath;
        public Dictionary<string, string> Users { get; set; }

        public Authenticator()
        {
            userFilePath = Config.LoadSetting(Config.userConfigKey);
            Users = new Dictionary<string, string>();

            LoadUserFile();
        }

        // Ajouter un utilisateur
        public bool AddUser(string pseudo, string password)
        {
            if (Users.ContainsKey(pseudo))
                return false;

            Users[pseudo] = password;
            SaveUserFile();
            return true;
        }

        // Supprimer un utilisateur
        public bool RemoveUser(string pseudo)
        {
            if (!Users.Remove(pseudo))
                return false;

            SaveUserFile();
            return true;
        }

        // Modifier le mot de passe
        public bool ChangePassword(string pseudo, string newPassword)
        {
            if (!Users.ContainsKey(pseudo))
                return false;

            Users[pseudo] = newPassword;
            SaveUserFile();
            return true;
        }

        // Vérifie si l'utilisateur existe
        public bool UserExists(string pseudo)
        {
            return Users.ContainsKey(pseudo);
        }

        // Authentifier un utilisateur
        public bool AuthenticateUser(string pseudo, string password)
        {
            return Users.TryGetValue(pseudo, out var storedPassword) && storedPassword == password;
        }

        // Récupérer le mot de passe (à éviter en production)
        public string? GetPassword(string pseudo)
        {
            return Users.TryGetValue(pseudo, out var pwd) ? pwd : null;
        }

        // Charger les données depuis le fichier JSON
        public void LoadUserFile()
        {
            if (!File.Exists(userFilePath))
                return;

            string json = File.ReadAllText(userFilePath);
            var users = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (users != null)
                Users = users;
        }

        // Sauvegarder les données dans le fichier JSON
        public void SaveUserFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(userFilePath, json);
        }
    }
}
