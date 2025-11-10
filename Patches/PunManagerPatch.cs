using HarmonyLib;
using UnityEngine;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PunManager))]
    internal class PunManagerPatch {

        [HarmonyPatch(nameof(PunManager.UpdateHealthRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateHealthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                playerAvatar.playerHealth.maxHealth = 100 + (StatsManager.instance.playerUpgradeHealth[_steamID] * 20);
                playerAvatar.playerHealth.health = playerAvatar.playerHealth.maxHealth - 1;
                playerAvatar.playerHealth.Heal(1, false);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateEnergyRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateEnergyRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.EnergyStart = 40 + (StatsManager.instance.playerUpgradeStamina[_steamID] * 10f);
                PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateExtraJumpRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateExtraJumpRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.JumpExtra = StatsManager.instance.playerUpgradeExtraJump[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateMapPlayerCountRightAway))]
        [HarmonyPrefix]
        private static bool UpdateMapPlayerCountRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                playerAvatar.upgradeMapPlayerCount = StatsManager.instance.playerUpgradeMapPlayerCount[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateTumbleLaunchRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleLaunchRightAway(ref string _steamID) {
            SemiFunc.PlayerAvatarGetFromSteamID(_steamID).tumble.tumbleLaunch = StatsManager.instance.playerUpgradeLaunch[_steamID];
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateSprintSpeedRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateSprintSpeedRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.SprintSpeed = 5f + (float) StatsManager.instance.playerUpgradeSpeed[_steamID];
                PlayerController.instance.SprintSpeedUpgrades = (float) StatsManager.instance.playerUpgradeSpeed[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateGrabStrengthRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateGrabStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabStrength = 1f + (StatsManager.instance.playerUpgradeStrength[_steamID] * 0.2f);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateThrowStrengthRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateThrowStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.throwStrength = (StatsManager.instance.playerUpgradeThrow[_steamID] * 0.3f);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateGrabRangeRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateGrabRangeRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabRange = 4 + (StatsManager.instance.playerUpgradeRange[_steamID] * 1f);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateCrouchRestRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateCrouchRestRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.upgradeCrouchRest = StatsManager.instance.playerUpgradeCrouchRest[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateTumbleWingsRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleWingsRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.upgradeTumbleWings = StatsManager.instance.playerUpgradeTumbleWings[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateDeathHeadBatteryRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateDeathHeadBatteryRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.upgradeDeathHeadBattery = StatsManager.instance.playerUpgradeDeathHeadBattery[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateTumbleClimbRightAway))]
        [HarmonyPrefix]
        private static bool Prefix_UpdateTumbleClimbRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.upgradeTumbleClimb = StatsManager.instance.playerUpgradeTumbleClimb[_steamID];
            }
            return false;
        }

    }
}
