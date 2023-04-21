using Rage;
using System.Windows.Forms;

namespace Polish_Callouts
{
    internal static class Settings
    {
        public static bool Alcohol_Public = true;

        public static Keys FinishCalloutKey;

        private static InitializationFile LocalizationFile;
        private static string Localization = "pl";

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

            Game.LogTrivial("[PLC] The .INI file has been loaded successfully.");
        }

        internal static string getStringFromLocalization(string section, string key)
        {
            return LocalizationFile.ReadString(section, key);
        }
    }
}
