using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage.Native;
using System;
using System.Collections.Generic;
using PolishCallouts.Usefuls;
using System.Drawing;
using System.Drawing.Text;

namespace Polish_Callouts.Callouts
{
    [CalloutInfo("[PLC] Spozywanie alkoholu w m. publicznym", CalloutProbability.High)]
    public class Alcohol_public : Callout
    {
        private Ped Suspect;
        private Vector3 spawnPoint;
        private Blip Blip;
        private Blip SuspectBlip;
        private protected int LocIndex;
        private protected int Scenario;
        private LHandle Pursuit;
        private Random rand = new Random();
        private bool _CalloutIsRunning;
        private Ped Player = Game.LocalPlayer.Character;

        private Vector4[] list =
        {
            new Vector4(-346.277161f, -364.4224f, 31.5574436f, 20),
            new Vector4(-251.366776f, -886.5289f, 30.66668f, 337),
            new Vector4(201.233871f, -997.1756f, 30.0919266f, 37),
            new Vector4(223.208466f, -912.827637f, 30.6920223f, 53),
            new Vector4(194.875687f, -862.3178f, 31.30845f, 4),
            new Vector4(43.40149f, -997.846863f, 29.3395119f, 343),
            new Vector4(-61.9648438f, -486.035431f, 40.41608f, 355),
            new Vector4(169.757874f, -221.3094f, 54.1841125f, 12),
            new Vector4(377.002625f, 325.554f, 103.566383f, 152),
            new Vector4(345.2516f, 351.283936f, 105.293846f, 265),
            new Vector4(282.065857f, 266.140625f, 105.603363f, 341),
            new Vector4(-196.7313f, 139.658615f, 70.16594f, 157),
            new Vector4(-700.2648f, 12.1518373f, 38.2552071f, 190),
            new Vector4(-863.971252f, -71.31679f, 37.86843f, 201),
            new Vector4(-1042.51514f, -148.3002f, 38.1421f, 130),
            new Vector4(-1267.223f, -299.7999f, 37.11333f, 120),
            new Vector4(-1675.554f, -461.306274f, 39.1278648f, 230),
            new Vector4(-1389.794f, -1332.36731f, 4.15015936f, 346),
            new Vector4(-1328.99829f, -1555.38245f, 4.372573f, 120),
            new Vector4(-1272.45044f, -1621.53174f, 4.4700284f, 40),
            new Vector4(-1147.79333f, -1613.396f, 4.387659f, 120),
            new Vector4(-990.044f, -1110.959f, 2.119371f, 110),
            new Vector4(-889.155334f, -852.9048f, 20.5660744f, 280),
            new Vector4(-510.691742f, -1211.95813f, 18.5265656f, 50),
            new Vector4(-822.6003f, -1083.875f, 11.1323805f, 222),
            new Vector4(-827.56665f, -923.494f, 16.5126438f, 233),
            new Vector4(131.204559f, -1352.36572f, 29.2023067f, 240),
            new Vector4(-77.57935f, -1320.40637f, 29.1787624f, 353),
            new Vector4(24.2871857f, -1514.6897f, 29.4140816f, 230),
            new Vector4(67.41986f, -1921.55859f, 21.364439f, 310),
            new Vector4(1581.20569f, 3644.45337f, 34.5143433f, 26),
            new Vector4(915.876038f, 3643.14f, 32.6439438f, 175)
        };

        private string[] pedModels = new string[] { "a_m_m_trampbeac_01", "a_m_m_tramp_01", "a_m_o_soucent_03", "a_m_m_genfat_01", "a_m_m_rurmeth_01", "a_m_m_salton_04", "a_m_y_cyclist_01", "a_m_m_mlcrisis_01", "a_m_y_stbla_02", "a_m_o_acult_02", "a_f_y_soucent_01", "a_f_m_skidrow_01", "g_f_y_families_01", "g_f_y_vagos_01", "g_f_y_ballas_01", "g_m_y_ballasout_01", "ig_floyd", "ig_trafficwarden" };
        private string[] weaponList = new string[] { "WEAPON_BOTTLE", "WEAPON_KNIFE", "WEAPON_SWITCHBLADE", "WEAPON_DAGGER", "WEAPON_UNARMED", "WEAPON_PISTOL" };
        
        public override bool OnBeforeCalloutDisplayed()
        {
            LocIndex = Usefuls.getNearestLocationIndex(list);
            spawnPoint = new Vector3(list[LocIndex].X, list[LocIndex].Y, list[LocIndex].Z);

            CalloutMessage = "[PLC] " + Settings.getStringFromLocalization("Callouts", "Alcohol_Public_Name");
            CalloutPosition = spawnPoint;
            CalloutAdvisory = Settings.getStringFromLocalization("Callouts", "Alcohol_Public_Advisory");
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 70f);

            Functions.PlayScannerAudioUsingPosition("UNITS_RESPOND_CODE_02_01 IN_OR_ON_POSITION", spawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[PLC] Alcohol_Public callout has been accepted.");

            int index = rand.Next(pedModels.Length);
            Suspect = new Ped(pedModels[index], spawnPoint, list[LocIndex].W);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            Blip = new Blip(spawnPoint.Around2D(1f, 5f), 70f);
            Blip.Color = System.Drawing.Color.Yellow;
            Blip.EnableRoute(System.Drawing.Color.Yellow);
            Blip.Alpha = 0.5f;

            Scenario = -1;
            NativeFunction.Natives.TASK_START_SCENARIO_IN_PLACE(Suspect, "WORLD_HUMAN_DRINKING", 0, false);

            _CalloutIsRunning = true;
            return base.OnCalloutAccepted();
        }
        public override void OnCalloutNotAccepted()
        {
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    if (_CalloutIsRunning)
                    { 
                        float distance = Player.DistanceTo(Suspect);

                        if (distance <= 25f)
                        {
                            if (!SuspectBlip.Exists() && Suspect.Exists())
                            {
                                if (Blip.Exists()) { Blip.Delete(); }

                                SuspectBlip = Suspect.AttachBlip();

                                Game.DisplayHelp(string.Format(Settings.getStringFromLocalization("HelpBox", "FinishCallout"), Settings.FinishCalloutKey));
                            }
                        } else { GameFiber.Wait(0); }

                        if (Scenario == -1 && !Player.IsInAnyVehicle(false))
                        {
                            if (distance <= 6f)
                            {
                                Scenario = 0;

                                int rand_scenario = rand.Next(100);
                                if (rand_scenario <= 40) // 40% szans na scenariusz
                                {
                                    int rand_pursuit = rand.Next(100);
                                    if (rand_pursuit <= 60) // 60% szansy na atak
                                    {
                                        Suspect.Inventory.GiveNewWeapon(weaponList[rand.Next(weaponList.Length)], -1, true);
                                        Suspect.RelationshipGroup = RelationshipGroup.Gang1;
                                        Suspect.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                                        Suspect.RelationshipGroup.SetRelationshipWith(Player.RelationshipGroup, Relationship.Hate);

                                        Suspect.Tasks.FightAgainstClosestHatedTarget(50, -1);

                                        Scenario = 1;
                                    }
                                    else // 40% szans na ucieczke
                                    {
                                        Pursuit = Functions.CreatePursuit();
                                        Functions.AddPedToPursuit(Pursuit, Suspect);
                                        Functions.SetPursuitIsActiveForPlayer(Pursuit, true);

                                        Scenario = 2;
                                    }
                                }

                                int drinkingChance = rand.Next(100);
                                if (drinkingChance <= 70) // 70% szans, że pijany
                                {
                                    NativeFunction.Natives.x95D2D383D5396B8A(Suspect, true);
                                    Suspect.MovementAnimationSet = "move_m@drunk@verydrunk";
                                }

                                GameFiber.Wait(2000);

                            } else { GameFiber.Wait(0); }
                        }

                        if (Game.IsKeyDown(Settings.FinishCalloutKey) || (Suspect.Exists() && (Suspect.IsDead || Functions.IsPedArrested(Suspect))) || Player.IsDead)
                        {
                            End();
                            GameFiber.Yield();
                        }
                    }
                } catch (Exception e)
                {
                    Game.LogTrivial("[PLC] PolishCallout ERROR!!!!");
                    Game.LogTrivial("ERROR OCCURED IN: ~y~" + this);
                    string error = e.ToString();
                    Game.LogTrivial("ERROR: ~r~" + e);

                    Game.DisplayNotification("[PLC] ~r~ERROR ~woccuerd, callout has been aborted. Check the ~g~.log ~w~file for more information");
                    End();
                }
            }, "[PLC] Alcohol_in_public");

            base.Process();
        }

        public override void End()
        {
            if (Suspect.Exists()) { Suspect.Dismiss(); }
            if (SuspectBlip.Exists()) { SuspectBlip.Delete(); }
            if (Blip.Exists()) { Blip.Delete(); }

            _CalloutIsRunning = false;

            if (!Player.IsDead)
                Functions.PlayScannerAudio("ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.DisplayNotification("~b~Code 4. ~w~No further units required");

            Game.LogTrivial("[PLC] Alcohol_Public callouts has been ended (scenario: " + Scenario.ToString() + ")");
            base.End();
        }
    }
}
