using HarmonyLib;
using Steamworks;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PunManager))]
    internal class PunManagerPatch {

        [HarmonyPatch(nameof(PunManager.UpdateHealthRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateHealthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                playerAvatar.playerHealth.maxHealth = 100 + (StatsManager.instance.playerUpgradeHealth[_steamID] * 20);
                playerAvatar.playerHealth.Heal(playerAvatar.playerHealth.maxHealth - playerAvatar.playerHealth.health, false);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateEnergyRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateEnergyRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.EnergyStart = 40 + (StatsManager.instance.playerUpgradeStamina[_steamID] * 10f);
                PlayerController.instance.EnergyCurrent = PlayerController.instance.EnergyStart;
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateExtraJumpRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateExtraJumpRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.JumpExtra = StatsManager.instance.playerUpgradeExtraJump[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateMapPlayerCountRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool UpdateMapPlayerCountRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar == SemiFunc.PlayerAvatarLocal()) {
                playerAvatar.upgradeMapPlayerCount = StatsManager.instance.playerUpgradeMapPlayerCount[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateTumbleLaunchRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateTumbleLaunchRightAway(ref string _steamID) {
            SemiFunc.PlayerAvatarGetFromSteamID(_steamID).tumble.tumbleLaunch = StatsManager.instance.playerUpgradeLaunch[_steamID];
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateSprintSpeedRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateSprintSpeedRightAway(ref string _steamID) {
            if (SemiFunc.PlayerAvatarGetFromSteamID(_steamID) == SemiFunc.PlayerAvatarLocal()) {
                PlayerController.instance.SprintSpeed = StatsManager.instance.playerUpgradeSpeed[_steamID];
                PlayerController.instance.SprintSpeedUpgrades = StatsManager.instance.playerUpgradeSpeed[_steamID];
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateGrabStrengthRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateGrabStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabStrength = 1f + (StatsManager.instance.playerUpgradeStrength[_steamID] * 0.2f);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateThrowStrengthRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateThrowStrengthRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.throwStrength = (StatsManager.instance.playerUpgradeThrow[_steamID] * 0.3f);
            }
            return false;
        }

        [HarmonyPatch(nameof(PunManager.UpdateGrabRangeRightAway))]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        private static bool Prefix_UpdateGrabRangeRightAway(ref string _steamID) {
            PlayerAvatar playerAvatar = SemiFunc.PlayerAvatarGetFromSteamID(_steamID);
            if (playerAvatar) {
                playerAvatar.physGrabber.grabRange = 4 + (StatsManager.instance.playerUpgradeRange[_steamID] * 1f);
            }
            return false;
        }

    }
}
