﻿using CCL.GTAIV;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

// Credits: ClonkAndre

namespace LibertyTweaks
{
    internal class BrakeLights
    {

        private static bool enable;

        public static void Init(SettingsFile settings)
        {
            enable = settings.GetBoolean("Fixes", "Brake Lights Fix", true);
        }

        public static void Tick()
        {
            if (!enable)
                return;

            // Gets the player ped
            IVPed playerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
            int playerId = playerPed.GetHandle();

            // Gets the current vehicle of the player
            IVVehicle playerVehicle = IVVehicle.FromUIntPtr(playerPed.GetVehicle());

            if (playerVehicle != null)
            {
                // Gets the speed of the current vehicle of the player
                GET_CAR_SPEED(playerVehicle.GetHandle(), out float carSpeed);

                // If speed of the vehicle is below 2f
                if (carSpeed < 1f)
                {
                    // Disable the brake lights if the player presses the gas pedal
                    if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || (NativeControls.IsUsingController() && NativeControls.IsGameKeyPressed(0, GameKey.Attack)))
                        playerVehicle.BrakePedal = 0f;
                    else // Activate brake lights
                        playerVehicle.BrakePedal = 0.15f;
                }
            }
        }

    }
}
