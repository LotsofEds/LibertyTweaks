﻿using IVSDKDotNet;

// Credits: catsmackaroo, ClonkAndre

namespace LibertyTweaks
{
    internal class TweakableFOV
    {
        private static bool enableFix;

        public static void Init(SettingsFile settings)
        {
            enableFix = settings.GetBoolean("Main", "Tweakable Field of View", true);
        }

        public static void Tick(float fovMulti)
        {
            if (!enableFix)
                return;

            IVCam cam = IVCamera.TheFinalCam;

            if (cam != null)
                cam.FOV = cam.FOV * fovMulti;
        }
    }
}
