using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace gameVaultClassLibrary
{
    public abstract class Config
    {
        public const string registryPath = @"Software\gameVault"; // App registry path
        public const string appDataKey = "appDataPath"; // Store the path to app data
        public const string userConfigKey = "userConfigFile"; // Store the file path of the user passwords file
        public const string librariesConfigKey = "librariesConfigFile"; // Store the file path of the libraries config file

        //public Config()
        //{
            
        //}

        public static void SetUpConfig()
        {
            // Get the user %APPDATA%\gameVault folder
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gameVault");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // Create appDateKey if it doesn't exist
            if (!SettingExists(appDataKey))
            {
                SaveSetting(appDataKey, appDataPath);
            }

            // Create userConfigKey if it doesn't exist
            if (!SettingExists(userConfigKey))
            {
                string userConfigFilePath = Path.Combine(appDataPath, "passwords.txt");

                SaveSetting(userConfigKey, userConfigFilePath);
            }

            // Create librariesConfigKey if it doesn't exist
            if (!SettingExists(librariesConfigKey))
            {
                string librariesConfigFilePath = Path.Combine(appDataPath, "libraries.txt");

                SaveSetting(librariesConfigKey, librariesConfigFilePath);
            }
        }

        // Create or update the setting
        public static void SaveSetting(string keyName, string value)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue(keyName, value);
            key.Close();
        }

        // Load the setting
        public static string LoadSetting(string keyName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key != null)
            {
                object value = key.GetValue(keyName);
                key.Close();
                return value?.ToString();
            }
            return null;
        }

        // Delete the setting
        public static void DeleteSetting(string keyName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, writable: true);
            if (key != null)
            {
                key.DeleteValue(keyName, false); // false = ne lève pas d'exception si la clé n'existe pas
                key.Close();
            }
        }

        // Check if the setting exists
        public static bool SettingExists(string keyName)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath))
            {
                return key?.GetValue(keyName) != null;
            }
        }
    }
}
