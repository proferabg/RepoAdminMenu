using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using REPOLib.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class PlayerUtil {

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
            FieldInfo maxHealthField = AccessTools.Field(typeof(PlayerHealth), "maxHealth");
            FieldInfo healthField = AccessTools.Field(typeof(PlayerHealth), "health");
            if (maxHealthField != null && healthField != null) {
                avatar.playerHealth.Heal((int)maxHealthField.GetValue(health), true);
                StatsManager.instance.SetPlayerHealth(SemiFunc.PlayerGetSteamID(avatar), (int)healthField.GetValue(health), false);
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Healed!");
            } else {
                RepoAdminMenu.mls.LogError("Failed to grab field 'PlayerHealth->maxHealth' or 'PlayerHealth->health'!");
            }
        }

        public static void upgrade(string type, PlayerAvatar avatar, int level) {
            StatsManager statsManager = StatsManager.instance;
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);
            FieldInfo photonViewField = AccessTools.Field(typeof(PunManager), "photonView");

            if (type.Equals("health") && statsManager.playerUpgradeHealth.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeHealth[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerHealthRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerHealthRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeHealth[playerSteamId]
                    });
                }
            } else if (type.Equals("jump") && statsManager.playerUpgradeExtraJump.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeExtraJump[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerExtraJumpRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerExtraJumpRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeExtraJump[playerSteamId]
                    });
                }
            } else if (type.Equals("launch") && statsManager.playerUpgradeLaunch.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeLaunch[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerTumbleLaunchRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerTumbleLaunchRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeLaunch[playerSteamId]
                    });
                }
            } else if (type.Equals("playercount") && statsManager.playerUpgradeMapPlayerCount.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeMapPlayerCount[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradeMapPlayerCountRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradeMapPlayerCountRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeMapPlayerCount[playerSteamId]
                    });
                }
            } else if (type.Equals("range") && statsManager.playerUpgradeRange.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeRange[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerGrabRangeRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerGrabRangeRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeRange[playerSteamId]
                    });
                }
            } else if (type.Equals("speed") && statsManager.playerUpgradeSpeed.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeSpeed[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerSprintSpeedRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerSprintSpeedRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeSpeed[playerSteamId]
                    });
                }
            } else if (type.Equals("stamina") && statsManager.playerUpgradeStamina.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeStamina[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerEnergyRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerEnergyRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeStamina[playerSteamId]
                    });
                }

            } else if (type.Equals("strength") && statsManager.playerUpgradeStrength.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeStrength[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerGrabStrengthRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerGrabStrengthRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeStrength[playerSteamId]
                    });
                }
            } else if (type.Equals("throw") && statsManager.playerUpgradeThrow.ContainsKey(playerSteamId)) {
                statsManager.playerUpgradeThrow[playerSteamId] = level;
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    PunManager.instance.UpgradePlayerThrowStrengthRPC(playerSteamId, level);
                }
                if (SemiFunc.IsMasterClient() && photonViewField != null) {
                    ((PhotonView)photonViewField.GetValue(PunManager.instance)).RPC("UpgradePlayerThrowStrengthRPC", RpcTarget.Others, new object[]
                    {
                    playerSteamId,
                    statsManager.playerUpgradeThrow[playerSteamId]
                    });
                }
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
