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

        public static void upgradeHealth(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeHealth[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerHealthRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerHealthRPC", RpcTarget.All, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeHealth[playerSteamId]
                });
            }
        }

        public static void upgradeJump(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeExtraJump[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerExtraJumpRPC(playerSteamId, level);
            }
            //multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerExtraJumpRPC", RpcTarget.All, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeExtraJump[playerSteamId]
                });
            }
        }

        public static void upgradeLaunch(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeLaunch[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerTumbleLaunchRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerTumbleLaunchRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeLaunch[playerSteamId]
                });
            }
        }

        public static void upgradeMapPlayerCount(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeMapPlayerCount[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradeMapPlayerCountRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradeMapPlayerCountRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeMapPlayerCount[playerSteamId]
                });
            }
        }

        public static void upgradeRange(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeRange[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerGrabRangeRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerGrabRangeRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeRange[playerSteamId]
                });
            }
        }

        public static void upgradeSpeed(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeSpeed[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerSprintSpeedRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerSprintSpeedRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeSpeed[playerSteamId]
                });
            }
        }

        public static void upgradeStamina(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeStamina[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerEnergyRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerEnergyRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeStamina[playerSteamId]
                });
            }
        }

        public static void upgradeStrength(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeStrength[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerGrabStrengthRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerGrabStrengthRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeStrength[playerSteamId]
                });
            }
        }

        public static void upgradeThrow(PlayerAvatar avatar, int level) {
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            // local
            StatsManager.instance.playerUpgradeThrow[playerSteamId] = level;
            if (SemiFunc.IsMasterClientOrSingleplayer()) {
                PunManager.instance.UpgradePlayerThrowStrengthRPC(playerSteamId, level);
            }
            // multiplayer
            if (SemiFunc.IsMasterClient()) {
                PunManager.instance.photonView.RPC("UpgradePlayerThrowStrengthRPC", RpcTarget.Others, new object[] {
                    playerSteamId,
                    StatsManager.instance.playerUpgradeThrow[playerSteamId]
                });
            }
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
