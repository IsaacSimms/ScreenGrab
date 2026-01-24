///// Class manages the saving and loading of persistent JSON file which holds the configuration settings        /////
///// includes hotkey assignments for screenshots. however, default hotkey configs are stored in HotkeyConfig.cs /////

// namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScreenGrab
{
    // class definition
    public static class ConfigurationManager
    {
        private static readonly string ConfigDirectory    = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScreenGrab"); // directory path // saves json to AppData\Roaming\ScreenGrab
        private static readonly string ConfigFileLocation = Path.Combine(ConfigDirectory, "config.json");                                                     // full file path

        // json serialization options
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true, // pretty print
            Converters    =       // convert enums to strings
            {
                new JsonStringEnumConverter()
            }
        };

        // load configuration from json file
        public static HotkeyConfig LoadConfiguration()
        {
            try
            {
                if (!File.Exists(ConfigFileLocation)) // check if config json file exists. If not, block gets run. // this will happen on first run of application
                {
                    var defaultConfig = new HotkeyConfig(); // create new instance of HotkeyConfig with default values
                    SaveConfiguration(defaultConfig);       // save default config to json file
                    return defaultConfig;                   // return default config
                }
                string jsonString = File.ReadAllText(ConfigFileLocation);                              // read json file contents
                var config        = JsonSerializer.Deserialize<HotkeyConfig>(jsonString, JsonOptions); // deserialize json to HotkeyConfig object
                return config     ?? new HotkeyConfig();                                               // return deserialized config or new default if null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}"); // log error to console
                return new HotkeyConfig();                                       // return new default config on error
            }
        }

        // save configuration to json file
        public static void SaveConfiguration(HotkeyConfig config)
        {
            try 
            {
                if (!Directory.Exists(ConfigDirectory)) // check if config directory exists
                {
                    Directory.CreateDirectory(ConfigDirectory);                                                             // create config directory if it doesn't exist
                }
                string jsonString = JsonSerializer.Serialize(config, JsonOptions);                                          // serialize HotkeyConfig object to json string
                File.WriteAllText(ConfigFileLocation, jsonString);                                                          // write json string to file
                System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(jsonString), "json file should never be empty"); // debug assertion for successful save
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving configuration: {ex.Message}");              // log error to console
                ScreenshotMessageBox.ShowMessage("Error saving configuration settings.", "ScreenGrab:", 4000); // show error message box to user
            }
        }
        // Get the configuration file path (for testing purposes)
        public static string GetConfigFilePath()
        {
            return ConfigFileLocation;
        }
    }
}