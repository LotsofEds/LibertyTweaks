﻿using System;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Wordprocessing;
using IVSDKDotNet;

namespace LibertyTweaks
{
    public class Main : Script
    {

        #region Variables
        private static Random rnd; 
        public float fovMulti;

        public int pedAccuracy;
        public int pedFirerate;

        public int armoredCopsStars;

        public int unseenSlipAwayMinTimer;
        public int unseenSlipAwayMaxTimer;

        public int regenHealthMinTimer;
        public int regenHealthMaxTimer;
        public int regenHealthMinHeal;
        public int regenHealthMaxHeal;

        public float recoilSmallPistolAmp1;
        public float recoilSmallPistolAmp2;
        public float recoilSmallPistolFreq1;
        public float recoilSmallPistolFreq2;

        public float recoilHeavyPistolAmp1;
        public float recoilHeavyPistolAmp2;
        public float recoilHeavyPistolFreq1;
        public float recoilHeavyPistolFreq2;

        public float recoilShotgunsAmp1;
        public float recoilShotgunsAmp2;
        public float recoilShotgunsFreq1;
        public float recoilShotgunsFreq2;

        public float recoilSMGAmp1;
        public float recoilSMGAmp2;
        public float recoilSMGFreq1;
        public float recoilSMGFreq2;

        public float recoilAssaultRiflesAmp1;
        public float recoilAssaultRiflesAmp2;
        public float recoilAssaultRiflesFreq1;
        public float recoilAssaultRiflesFreq2;

        public DateTime timer;

        private Keys quickSaveKey;
        private Keys holsterKey;
        private Keys toggleHudKey;
        private Keys quickGpsKey;

        public Keys positiveTalkKey;
        public Keys negativeTalkKey;

        private static CustomIVSave saveGame;
        #endregion

        #region Functions
        public static int GenerateRandomNumber(int x, int y)
        {
            return rnd.Next(x, y);
        }
        internal static CustomIVSave GetTheSaveGame()
        {
            return saveGame;
        }
        #endregion

        #region Constructor
        public Main()
        {
            rnd = new Random();

            Initialized += Main_Initialized;
            Tick += Main_Tick;
            Drawing += Main_Drawing;
            KeyDown += Main_KeyDown;
            ProcessAutomobile += Main_ProcessAutomobile;
            ProcessCamera += Main_ProcessCamera;
            IngameStartup += Main_IngameStartup;
            WaitTick +=Main_WaitTick;
            GameLoad += Main_GameLoad;
            GameLoadPriority += Main_GameLoadPriority;
        }

        private void Main_Drawing(object sender, EventArgs e)
        {
            NoCursorEscape.Process();
        }

        private void Main_GameLoadPriority(object sender, EventArgs e)
        {
            WeaponMagazines.LoadFiles();
            ArmoredCops.LoadFiles();
        }

        private void Main_WaitTick(object sender, EventArgs e)
        {
            WaitTickInterval=2000;
            UnholsteredGunFix.WaitTick();
        }

        private void Main_GameLoad(object sender, EventArgs e)
        {
            //QuickSave.Spawn();
        }
        #endregion

        private void Main_IngameStartup(object sender, EventArgs e)
        {
            QuickSave.IngameStartup();
        }

        private void Main_Initialized(object sender, EventArgs e)
        {
            // Check .INI
            // MAIN
            HolsterWeapons.Init(Settings);
            HigherPedAccuracy.Init(Settings);
            WeaponMagazines.Init(Settings);
            MoveWithSniper.Init(Settings);
            RemoveWeapons.Init(Settings);
            TweakableFOV.Init(Settings);
            QuickSave.Init(Settings);
            AutosaveOnCollectibles.Init(Settings);
            MoreCombatLines.Init(Settings);
            SearchBody.Init(Settings);
            VLikeScreaming.Init(Settings);
            ArmoredCops.Init(Settings);
            UnseenSlipAway.Init(Settings);
            RegenerateHP.Init(Settings);
            ToggleHUD.Init(Settings);
            Recoil.Init(Settings);
            CarFireBreakdown.Init(Settings);
            RealisticReloading.Init(Settings);
            //StunPunch.Init(Settings);
            //RandomNoEuphoria.Init(Settings);
            CopShotgunFix.Init(Settings);
            QuickGPS.Init(Settings);

            // FIXES
            NoOvertaking.Init(Settings);
            NoCursorEscape.Init(Settings);
            IceCreamSpeechFix.Init(Settings);
            WheelFix.Init(Settings);
            UnholsteredGunFix.Init(Settings);
            BrakeLights.Init(Settings);
            ExtraHospitalSpawn.Init(Settings);

            // SAVE
            saveGame = CustomIVSave.CreateOrLoadSaveGameData(this);

            // KEYS
            quickSaveKey = Settings.GetKey("Quick-Saving", "Key", Keys.F9);
            holsterKey = Settings.GetKey("Weapon Holstering", "Key", Keys.H);
            quickGpsKey = Settings.GetKey("Quick GPS", "Key", Keys.N);
            toggleHudKey = Settings.GetKey("Toggle HUD", "Key", Keys.K);

            // INTS
            pedAccuracy = Settings.GetInteger("Improved AI", "Accuracy", 85);
            pedFirerate = Settings.GetInteger("Improved AI", "Firerate", 85);

            armoredCopsStars = Settings.GetInteger("Improved Police", "Armored Cops Start At", 4);

            unseenSlipAwayMinTimer = Settings.GetInteger("Improved Police", "Lose Stars While Unseen Minimum Count", 60);
            unseenSlipAwayMaxTimer = Settings.GetInteger("Improved Police", "Lose Stars While Unseen Maximum Count", 120);

            regenHealthMinTimer = Settings.GetInteger("Health Regeneration", "Regen Timer Minimum", 30);
            regenHealthMaxTimer = Settings.GetInteger("Health Regeneration", "Regen Timer Maximum", 60);
            regenHealthMinHeal = Settings.GetInteger("Health Regeneration", "Minimum Heal Amount", 5);
            regenHealthMaxHeal = Settings.GetInteger("Health Regeneration", "Maximum Heal Amount", 10);

            // FLOATS
            fovMulti = Settings.GetFloat("Tweakable FOV", "Multiplier", 1.07f);
            recoilSmallPistolAmp1 = Settings.GetFloat("Extensive Settings", "Pistol Amplitude 1", 0.4f);
            recoilSmallPistolAmp2 = Settings.GetFloat("Extensive Settings", "Pistol Amplitude 2", 0.6f);
            recoilSmallPistolFreq1 = Settings.GetFloat("Extensive Settings", "Pistol Frequency 1", 0.1f);
            recoilSmallPistolFreq2 = Settings.GetFloat("Extensive Settings", "Pistol Frequency 2", 0.3f);

            recoilHeavyPistolAmp1 = Settings.GetFloat("Extensive Settings", "Heavy Pistol Amplitude 2", 0.2f);
            recoilHeavyPistolAmp2 = Settings.GetFloat("Extensive Settings", "Heavy Pistol Ampltitude 2", 0.4f);
            recoilHeavyPistolFreq1 = Settings.GetFloat("Extensive Settings", "Heavy Pistol Frequency 1", 0.3f);
            recoilHeavyPistolFreq2 = Settings.GetFloat("Extensive Settings", "Heavy Pistol Frequency 2", 0.5f);

            recoilShotgunsAmp1 = Settings.GetFloat("Extensive Settings", "Shotgun Amplitude 1", 0.3f);
            recoilShotgunsAmp2 = Settings.GetFloat("Extensive Settings", "Shotgun Amplitude 2", 0.7f);
            recoilShotgunsFreq1 = Settings.GetFloat("Extensive Settings", "Shotgun Frequency 1", 0.4f);
            recoilShotgunsFreq2 = Settings.GetFloat("Extensive Settings", "Shotgun Frequency 2", 0.7f);

            recoilSMGAmp1 = Settings.GetFloat("Extensive Settings", "SMG Amplitude 1", 0.4f);
            recoilSMGAmp2 = Settings.GetFloat("Extensive Settings", "SMG Amplitude 2", 0.6f);
            recoilSMGFreq1 = Settings.GetFloat("Extensive Settings", "SMG Frequency 1", 0.1f);
            recoilSMGFreq2 = Settings.GetFloat("Extensive Settings", "SMG Frequency 2", 0.3f);

            recoilAssaultRiflesAmp1 = Settings.GetFloat("Extensive Settings", "Assault Rifle Amplitude 1", 0.4f);
            recoilAssaultRiflesAmp2 = Settings.GetFloat("Extensive Settings", "Assault Rifle Amplitude 2", 0.6f);
            recoilAssaultRiflesFreq1 = Settings.GetFloat("Extensive Settings", "Assault Rifle Frequency 1", 0.1f);
            recoilAssaultRiflesFreq2 = Settings.GetFloat("Extensive Settings", "Assault Rifle Frequency 2", 0.6f);

            //positiveTalkKey = Settings.GetKey("Interactive NPCs", "Positive Speech", Keys.Y);
            //negativeTalkKey = Settings.GetKey("Interactive NPCs", "Positive Speech", Keys.N);
        }

        private void Main_ProcessCamera(object sender, EventArgs e)
        {
            TweakableFOV.Tick(fovMulti);
        }

        private void Main_ProcessAutomobile(UIntPtr vehPtr)
        {
            WheelFix.Process(vehPtr);
        }

        private void Main_Tick(object sender, EventArgs e)
        {
            // MAIN
            RemoveWeapons.Tick();
            HigherPedAccuracy.Tick(pedAccuracy, pedFirerate);
            WeaponMagazines.Tick();
            MoveWithSniper.Tick();
            MoreCombatLines.Tick();
            SearchBody.Tick();
            VLikeScreaming.Tick();
            ArmoredCops.Tick(armoredCopsStars);
            UnseenSlipAway.Tick(timer, unseenSlipAwayMinTimer, unseenSlipAwayMaxTimer);
            RegenerateHP.Tick(timer, regenHealthMinTimer, regenHealthMaxTimer, regenHealthMinHeal, regenHealthMaxHeal);
            CarFireBreakdown.Tick();
            Recoil.Tick(recoilSmallPistolAmp1,recoilSmallPistolAmp2,recoilSmallPistolFreq1, recoilSmallPistolFreq2, 
             recoilHeavyPistolAmp1, recoilHeavyPistolAmp2, recoilHeavyPistolFreq1, recoilHeavyPistolFreq2, 
             recoilShotgunsAmp1, recoilShotgunsAmp2, recoilShotgunsFreq1, recoilShotgunsFreq2, 
             recoilSMGAmp1, recoilSMGAmp2, recoilSMGFreq1, recoilSMGFreq2,
             recoilAssaultRiflesAmp1, recoilAssaultRiflesAmp2, recoilAssaultRiflesFreq1, recoilAssaultRiflesFreq2);
            RealisticReloading.Tick();
            QuickSave.Tick();
            NoOvertaking.Tick();
            //StunPunch.Tick(timer);
            //RandomNoEuphoria.Tick();


            // FIXES
            BrakeLights.Tick();
            CopShotgunFix.Tick();
            ExtraHospitalSpawn.Tick();
            IceCreamSpeechFix.Tick();
            WheelFix.PreChecks();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == toggleHudKey)
            {
                ToggleHUD.Process();
            }

            if (e.KeyCode == quickSaveKey)
            {
                QuickSave.Process();
            }

            if (e.KeyCode == holsterKey)
            {
                HolsterWeapons.Process();
            }

            // gillian: this is retarded and used for quickgps im sorry
            if (e.KeyCode == quickGpsKey)
            {
                IVGame.ShowSubtitleMessage("Hold GPS Key+number button to pick a destination: 1: Restaurant; 2: Safehouse; 3: Weapons; 4: Pay'N'Spray; 5: Internet; 6: Clothing; 7: Missions; 8: Helitour; 9: Entertainment; 0: Clear");
            }

            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    QuickGPS.Process(1, quickGpsKey);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    QuickGPS.Process(2, quickGpsKey);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    QuickGPS.Process(3, quickGpsKey);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    QuickGPS.Process(4, quickGpsKey);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    QuickGPS.Process(5, quickGpsKey);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    QuickGPS.Process(6, quickGpsKey);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    QuickGPS.Process(7, quickGpsKey);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    QuickGPS.Process(8, quickGpsKey);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    QuickGPS.Process(9, quickGpsKey);
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    QuickGPS.Process(0, quickGpsKey);
                    break;
            }
        }
    }
}
