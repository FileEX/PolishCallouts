using System.Reflection;
using Rage;
using LSPD_First_Response.Mod.API;
using Polish_Callouts.Callouts;

namespace Polish_Callouts
{
    public class Main : Plugin
    {
        public override void Finally() { }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyStateChangedEvent;
            Game.LogTrivial("[PLC] Polish Callouts v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() +" has been initalized!");
            
            Settings.Load();
        }

        private static void OnDutyStateChangedEvent(bool OnDuty)
        {
            if (OnDuty)
            {
                GameFiber.StartNew(delegate {
                    RegisterCallouts();

                    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    Game.Console.Print("[PLC] PolishCallouts v" + version + " has been loaded.");
                    Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "PolishCallouts", "~y~v" + version + " ~o~by FileEX", "~w~has been loaded ~g~successfully");

                    // check for updates TODO

                    GameFiber.Wait(300);
                });
            }
        }

        private static void RegisterCallouts()
        {
            Game.Console.Print("[PLC] Registering callouts...");

            if (Settings.Alcohol_Public) Functions.RegisterCallout(typeof(Alcohol_public));

            Game.LogTrivial("[PLC] Registering callouts from .ini file has been ended successfully");
        }
    }
}