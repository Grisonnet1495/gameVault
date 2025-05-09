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
        #region Properties
        public const string registryPath = @"Software\gameVault"; // App registry path
        public const string appDataKey = "appDataPath"; // Store the path to app data
        public const string userConfigKey = "userConfigFile"; // Store the file path of the user passwords file
        public const string librariesConfigKey = "librariesConfigFile"; // Store the file path of the libraries config file
        public const string imagesFolderKey = "imageFolder"; // Store the file path of the images folder 
        #endregion

        #region Constructor
        public static void SetUpConfig()
        {
            string appDataPath;

            // Load or create the appDataKey it if it doesn't exist
            if (!SettingExists(appDataKey))
            {
                // Get the user %APPDATA%\gameVault folder
                appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gameVault");

                SaveSetting(appDataKey, appDataPath);
            }
            else
            {
                appDataPath = LoadSetting(appDataKey);
            }

            // Create the app data directory if it doesn't exit
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // Create userConfigKey if it doesn't exist
            if (!SettingExists(userConfigKey))
            {
                string userConfigFilePath = "passwords.json";

                SaveSetting(userConfigKey, userConfigFilePath);
            }

            // Create librariesConfigKey if it doesn't exist
            if (!SettingExists(librariesConfigKey))
            {
                string librariesConfigFilePath = "libraries.json";

                SaveSetting(librariesConfigKey, librariesConfigFilePath);
            }

            string imagesFolderFilePath;

            // Create the imageFolderKey if it doesn't exist
            if (!SettingExists(imagesFolderKey))
            {
                string imagesFolderName = "images";
                imagesFolderFilePath = Path.Combine(LoadSetting(appDataKey), imagesFolderName);

                SaveSetting(imagesFolderKey, imagesFolderName);
            }
            else
            {
                imagesFolderFilePath = Path.Combine(LoadSetting(appDataKey), LoadSetting(imagesFolderKey));
            }

            // Create the app data directory if it doesn't exit
            if (!Directory.Exists(imagesFolderFilePath))
            {
                Directory.CreateDirectory(imagesFolderFilePath);
            }

            // Create the images directory if it doesn't exit
            if (!Directory.Exists(Config.LoadSetting(imagesFolderKey)))
            {
                Directory.CreateDirectory(Config.LoadSetting(imagesFolderKey));
            }
        }
        #endregion

        #region Methods
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
        #endregion
    }
}
