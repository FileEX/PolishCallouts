using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PolishCallouts
{
    internal static class Settings
    {
        public static bool Alcohol_Public = true;

        public static Keys FinishCalloutKey;

        private static InitializationFile LocalizationFile;
        private static string Localization = "pl";

        public static bool STP = false;

        internal static void Load()
        {
            string path = "Plugins/LSPDFR/PolishCallouts/PolishCallouts.ini";
            InitializationFile iniFile = new InitializationFile(path);
            iniFile.Create();

            // callouts
            Alcohol_Public = iniFile.ReadBoolean("Callouts", "Alcohol_Public", true);

            // settings
            FinishCalloutKey = iniFile.ReadEnum<Keys>("Keys", "FinishCallout", Keys.End);

            // localization
            Localization = iniFile.ReadString("Settings", "Language", "pl");

            LocalizationFile = new InitializationFile($"Plugins/LSPDFR/PolishCallouts/lang/{Localization}.ini");
            LocalizationFile.Create();

            STP = isPluginLoaded("Stop The Ped");

            Game.LogTrivial("[PLC] The .INI file has been loaded successfully.");
        }

        internal static string getStringFromLocalization(string section, string key)
        {
            return LocalizationFile.ReadString(section, key);
        }

        private static Func<string, bool> isPluginLoaded = pluginName => Functions.GetAllUserPlugins().Any(assembly => assembly.GetName().Name.Equals(pluginName));
    }
}
