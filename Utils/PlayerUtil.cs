using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class PlayerUtil {

        private static FieldInfo maxHealthField = AccessTools.Field(typeof(PlayerHealth), "maxHealth");
        private static FieldInfo healthField = AccessTools.Field(typeof(PlayerHealth), "health");

        public static void killPlayer(PlayerAvatar avatar) {
            avatar.PlayerDeath(-1);
            RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Killed!");
        }

        public static void revivePlayer(PlayerAvatar avatar) {
            FieldInfo deadSetField = AccessTools.Field(typeof(PlayerAvatar), "deadSet");
            if (deadSetField != null && (bool)deadSetField.GetValue(avatar) && avatar.playerDeathHead != null) {
                PlayerDeathHead playerDeathHead = avatar.playerDeathHead;
                FieldInfo inExtractionPointField = AccessTools.Field(typeof(PlayerDeathHead), "inExtractionPoint");
                if (inExtractionPointField != null) {
                    inExtractionPointField.SetValue(playerDeathHead, true);
                    avatar.Revive();
                    inExtractionPointField.SetValue(playerDeathHead, false);
                    RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Revived!");
                } else {
                    RepoAdminMenu.mls.LogError("Failed to grab field 'PlayerDeathHead->inExtractionPoint'!");
                }
            } else {
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " is not dead. Cannot be revived!");
            }
        }

        public static void healPlayer(PlayerAvatar avatar) {
            PlayerHealth health = avatar.playerHealth;
            if (maxHealthField != null && healthField != null) {
                avatar.playerHealth.HealOther((int)maxHealthField.GetValue(health), true);
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Healed!");
            } else {
                RepoAdminMenu.mls.LogError("Failed to grab field 'PlayerHealth->maxHealth' or 'PlayerHealth->health'!");
            }
        }

        // Each of these MUST be synced with StatsManager.Start()!
        [SuppressMessage("ReSharper", "InconsistentNaming")] 
        private enum StatType
        {
            playerUpgradeHealth,
            playerUpgradeStamina,
            playerUpgradeExtraJump,
            playerUpgradeLaunch,
            playerUpgradeMapPlayerCount,
            playerUpgradeSpeed,
            playerUpgradeStrength,
            playerUpgradeRange,
            playerUpgradeThrow,
            playerUpgradeCrouchRest,
            playerUpgradeTumbleWings
        }
        private static void SendStatUpdate(StatType stat, string playerSteamId, int level)
        {
            var statID = Enum.GetName(typeof(StatType), stat);
            // Was used for debugging
            // RepoAdminMenu.mls.LogInfo($"Sending stat update: {statID}, {playerSteamId}, {level}");
            PunManager.instance.UpdateStat(statID,playerSteamId,level);
        }

        public static void upgradeHealth(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeHealth, playerSteamId, level);
            // Re-implement UpgradeHealthRightAway
            avatar.playerHealth.maxHealth = 100+StatsManager.instance.GetPlayerMaxHealth(playerSteamId);
            avatar.playerHealth.health = Mathf.Clamp(avatar.playerHealth.health,0,avatar.playerHealth.maxHealth);
            // Send changes to player
            StatsManager.instance.SetPlayerHealth(playerSteamId, avatar.playerHealth.health, false);
            if (GameManager.Multiplayer())
            {
                avatar.playerHealth.photonView.RPC("UpdateHealthRPC", RpcTarget.Others, new object[] { avatar.playerHealth.health, avatar.playerHealth.maxHealth, true });
            }
        }


        public static void upgradeJump(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeExtraJump, playerSteamId, level);
            // Re-implement UpgradeExtraJumpRightAway
            // FIXME: Only works locally.
            if (avatar == SemiFunc.PlayerAvatarLocal())
            {
                PlayerController.instance.JumpExtra = level;
            }
        }

        public static void upgradeLaunch(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeLaunch, playerSteamId, level);
            // Re-implement UpgradeTumbleLaunchRightAway
            avatar.tumble.tumbleLaunch = level;
        }

        public static void upgradeMapPlayerCount(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeMapPlayerCount, playerSteamId, level);
            // Re-implement UpgradeMapPlayerCountRightAway
            avatar.upgradeMapPlayerCount = level;
        }

        public static void upgradeRange(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeRange, playerSteamId, level);
            // Re-implement UpgradeGrabRangeRightAway
            avatar.physGrabber.grabRange = 4f+(level*1f);
        }

        public static void upgradeSpeed(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeSpeed, playerSteamId, level);
            // Re-implement UpgradeGrabRangeRightAway
            // FIXME: Only works locally.
            if (avatar == SemiFunc.PlayerAvatarLocal())
            {
                PlayerController.instance.SprintSpeed = 1f+level*1f;
                PlayerController.instance.SprintSpeedUpgrades = level*1f;
            }
        }

        public static void upgradeStamina(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeStamina, playerSteamId, level);
            if (avatar == SemiFunc.PlayerAvatarLocal())
            {
                PlayerController.instance.EnergyStart = 100f + level * 10f;
                PlayerController.instance.EnergyCurrent = Mathf.Clamp(PlayerController.instance.EnergyCurrent,0f,PlayerController.instance.EnergyStart);
            }
        }

        public static void upgradeStrength(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeStrength, playerSteamId, level);
            avatar.physGrabber.grabStrength = 1f+level*0.2f;
        }

        public static void upgradeThrow(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeThrow, playerSteamId, level);
            avatar.physGrabber.throwStrength = level*0.3f;
        }

        public static void upgradeTumbleWings(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeTumbleWings, playerSteamId, level);
            // avatar.upgradeTumbleWings = level * 1f;
            // HACK: Temporary, until NuGet packages update
            typeof(PlayerAvatar).GetField("upgradeTumbleWings", BindingFlags.Instance | BindingFlags.Public|BindingFlags.NonPublic).SetValue(avatar,level*1f);
        }

        public static void upgradeCrouchRest(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            SendStatUpdate(StatType.playerUpgradeCrouchRest, playerSteamId, level);
            // avatar.upgradeCrouchRest = level * 1f;
            // HACK: Temporary, until NuGet packages update
            typeof(PlayerAvatar).GetField("upgradeCrouchRest", BindingFlags.Instance | BindingFlags.Public|BindingFlags.NonPublic).SetValue(avatar,level*1f);
        }

        public static int getUpgradeLevel(string type, PlayerAvatar avatar) {
            StatsManager statsManager = StatsManager.instance;
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);

            if (type.Equals("health") && statsManager.playerUpgradeHealth.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeHealth[playerSteamId];
            } else if (type.Equals("jump") && statsManager.playerUpgradeExtraJump.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeExtraJump[playerSteamId];
            } else if (type.Equals("launch") && statsManager.playerUpgradeLaunch.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeLaunch[playerSteamId];
            } else if (type.Equals("playercount") && statsManager.playerUpgradeMapPlayerCount.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeMapPlayerCount[playerSteamId];
            } else if (type.Equals("range") && statsManager.playerUpgradeRange.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeRange[playerSteamId];
            } else if (type.Equals("speed") && statsManager.playerUpgradeSpeed.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeSpeed[playerSteamId];
            } else if (type.Equals("stamina") && statsManager.playerUpgradeStamina.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeStamina[playerSteamId];
            } else if (type.Equals("strength") && statsManager.playerUpgradeStrength.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeStrength[playerSteamId];
            } else if (type.Equals("throw") && statsManager.playerUpgradeThrow.ContainsKey(playerSteamId)) {
                return statsManager.playerUpgradeThrow[playerSteamId];
            // NOTE: dictionaryOfDictionaries contains references to the appropriate fields.
            } else if (type.Equals("tumblewings") && statsManager.dictionaryOfDictionaries["playerUpgradeTumbleWings"].ContainsKey(playerSteamId)) {
                return statsManager.dictionaryOfDictionaries["playerUpgradeTumbleWings"][playerSteamId];
            } else if (type.Equals("crouchrest") && statsManager.dictionaryOfDictionaries["playerUpgradeCrouchRest"].ContainsKey(playerSteamId)) {
                return statsManager.dictionaryOfDictionaries["playerUpgradeCrouchRest"][playerSteamId];
            }

            return 0;
        }

        public static void giveCrown(PlayerAvatar avatar) {
            SessionManager.instance.crownedPlayerSteamID = SemiFunc.PlayerGetSteamID(avatar);
            SessionManager.instance.CrownPlayer();
        }

        public static void teleportTo(PlayerAvatar avatar) {
            teleport(PlayerAvatar.instance, avatar);
        }

        public static void summon(PlayerAvatar avatar) {
            teleport(avatar, PlayerAvatar.instance);
        }

        private static void teleport(PlayerAvatar from, PlayerAvatar to) {
            Vector3 pos = to.transform.position;
            Quaternion rot = to.transform.rotation;
            if (to.deadSet) {
                pos = to.playerDeathHead.physGrabObject.transform.position;
                rot = to.playerDeathHead.physGrabObject.transform.rotation;
            }
            if (!SemiFunc.IsMultiplayer()) {
                from.Spawn(pos, rot);
            } else if (from.photonView != null){
                if (from.deadSet) {
                    from.playerDeathHead.physGrabObject.Teleport(pos, rot);
                } else {
                    from.photonView.RPC("SpawnRPC", RpcTarget.All, new object[] { pos, rot });
                }
            }
        }

        public static void KickPlayer(PlayerAvatar avatar) {
            // no kicking host
            if (avatar == PlayerAvatar.instance) {
                return;
            }

            //avatar.photonView.RPC("OutroStartRPC", RpcTarget.All, new object[] { });
            RepoAdminMenu.mls.LogInfo("Sending kick to: " + SemiFunc.PlayerGetName(avatar));
            NetworkUtil.SendCommandSteamIDString("KickPlayer", avatar.steamID, "", ReceiverGroup.Others);
        }
    }
}
