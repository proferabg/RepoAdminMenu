using HarmonyLib;
using UnityEngine;
using RepoAdminMenu.Utils;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PunManager))]
    internal class PunManagerPatch {

        [HarmonyPatch("UpdateHealthRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateHealthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                ReflectionUtil.PlayerHealth_SetMaxHealth(playerAvatar.playerHealth, 100 + (StatsManager.instance.playerUpgradeHealth[_steamID] * 20));
                ReflectionUtil.PlayerHealth_SetHealth(playerAvatar.playerHealth, ReflectionUtil.PlayerHealth_GetMaxHealth(playerAvatar.playerHealth) - 1);
                playerAvatar.playerHealth.Heal(1, false);
            }
            return false;
        }

        [HarmonyPatch("UpdateEnergyRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateEnergyRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.EnergyStart = 40 + (StatsManager.instance.playerUpgradeStamina[_steamID] * 10f);
                PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;
            }
            return false;
        }

        [HarmonyPatch("UpdateExtraJumpRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateExtraJumpRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                ReflectionUtil.PlayerController_SetJumpExtra(PlayerController.instance, StatsManager.instance.playerUpgradeExtraJump[_steamID]);
            }
            return false;
        }

        [HarmonyPatch("UpdateMapPlayerCountRightAway")]
        [HarmonyPrefix]
        private static bool UpdateMapPlayerCountRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                ReflectionUtil.PlayerAvatar_SetUpgradeMapPlayerCount(playerAvatar, StatsManager.instance.playerUpgradeMapPlayerCount[_steamID]);
            }
            return false;
        }

        [HarmonyPatch("UpdateTumbleLaunchRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleLaunchRightAway(ref string _steamID) {
            ReflectionUtil.SetFieldValue(ReflectionUtil.PlayerAvatar_GetTumble(SemiFunc.PlayerAvatarGetFromSteamID(_steamID)), "tumbleLaunch", StatsManager.instance.playerUpgradeLaunch[_steamID]);
            return false;
        }

        [HarmonyPatch("UpdateSprintSpeedRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateSprintSpeedRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.SprintSpeed = 5f + (float) StatsManager.instance.playerUpgradeSpeed[_steamID];
                PlayerController.instance.SprintSpeedUpgrades = (float) StatsManager.instance.playerUpgradeSpeed[_steamID];
            }
            return false;
        }

        [HarmonyPatch("UpdateGrabStrengthRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateGrabStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabStrength = 1f + (StatsManager.instance.playerUpgradeStrength[_steamID] * 0.2f);
            }
            return false;
        }

        [HarmonyPatch("UpdateThrowStrengthRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateThrowStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.throwStrength = (StatsManager.instance.playerUpgradeThrow[_steamID] * 0.3f);
            }
            return false;
        }

        [HarmonyPatch("UpdateGrabRangeRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateGrabRangeRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabRange = 4 + (StatsManager.instance.playerUpgradeRange[_steamID] * 1f);
            }
            return false;
        }

        [HarmonyPatch("UpdateCrouchRestRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateCrouchRestRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                ReflectionUtil.PlayerAvatar_SetUpgradeCrouchRest(playerAvatar, StatsManager.instance.playerUpgradeCrouchRest[_steamID]);
            }
            return false;
        }

        [HarmonyPatch("UpdateTumbleWingsRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleWingsRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                ReflectionUtil.PlayerAvatar_SetUpgradeTumbleWings(playerAvatar, StatsManager.instance.playerUpgradeTumbleWings[_steamID]);
            }
            return false;
        }

        [HarmonyPatch("UpdateDeathHeadBatteryRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateDeathHeadBatteryRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                ReflectionUtil.PlayerAvatar_SetUpgradeDeathHeadBattery(playerAvatar, StatsManager.instance.playerUpgradeDeathHeadBattery[_steamID]);
            }
            return false;
        }

        [HarmonyPatch("UpdateTumbleClimbRightAway")]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleClimbRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                ReflectionUtil.PlayerAvatar_SetUpgradeTumbleClimb(playerAvatar, StatsManager.instance.playerUpgradeTumbleClimb[_steamID]);
            }
            return false;
        }

    }
}
