using Rage;

namespace Polish_Callouts
{
    internal static class Settings
    {
        internal static bool Alcohol_Public = true;

        internal static InitializationFile LocalizationFile;
        internal static string Localization = "pl";

        internal static void Load()
        {
            string path = "Plugins/LSPDFR/PolishCallouts/PolishCallouts.ini";
            InitializationFile iniFile = new InitializationFile(path);
            iniFile.Create();

            Alcohol_Public = iniFile.ReadBoolean("Callouts", "Alcohol_Public", true);

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
