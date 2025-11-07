using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace RepoAdminMenu.Utils {
    internal class PlayerUtil {

        private DateTime lastSync = DateTime.Now;

        public static void killPlayer(PlayerAvatar avatar) {
            avatar.PlayerDeath(-1);
            RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Killed!");
        }

        public static void revivePlayer(PlayerAvatar avatar) {
            if (avatar.deadSet && avatar.playerDeathHead != null) {
                PlayerDeathHead playerDeathHead = avatar.playerDeathHead;
                playerDeathHead.inExtractionPoint = true;
                avatar.Revive();
                playerDeathHead.inExtractionPoint = false;
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Revived!");
            } else {
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " is not dead. Cannot be revived!");
            }
        }

        public static void healPlayer(PlayerAvatar avatar) {
            PlayerHealth health = avatar.playerHealth;
            avatar.playerHealth.HealOther(health.maxHealth, true);
            RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Healed!");
        }

        public static void upgrade(PlayerAvatar avatar, string key, int level) {
            RepoAdminMenu.mls.LogInfo("Upgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + key + "' -> " + level);
            NetworkUtil.SendCommandSteamIDStringInt("UpgradeSync", avatar.steamID, key, level, ReceiverGroup.All);
        }

        public static void upgradeSync(PlayerAvatar avatar, string key, int level) {
            RepoAdminMenu.mls.LogInfo("UpgradeSync: " + SemiFunc.PlayerGetName(avatar) + " - '" + key + "' -> " + level);
            string steamId = SemiFunc.PlayerGetSteamID(avatar);
            Dictionary<string, Dictionary<string, int>> dicts = StatsManager.instance.dictionaryOfDictionaries;

            if (!dicts.ContainsKey(key)) {
                dicts.Add(key, new Dictionary<string, int>());
            }

            Dictionary<string, int> dict = new Dictionary<string, int>();

            if (dicts.TryGetValue(key, out dict)) {
                dict[steamId] = level;
            }

            // process upgrade locally
            if (avatar == PlayerAvatar.instance) {
                DoImmediateUpgrade(avatar, key, level);
            }
        }

        public static void DoImmediateUpgrade(PlayerAvatar avatar, string key, int level) {
            string steamId = SemiFunc.PlayerGetSteamID(avatar);
            RepoAdminMenu.mls.LogInfo("DoImmediateUpgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + key + "'");
            if (key.Equals("playerUpgradeHealth")) {
                PunManager.instance.UpdateHealthRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeStamina")) {
                PunManager.instance.UpdateEnergyRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeExtraJump")) {
                PunManager.instance.UpdateExtraJumpRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeLaunch")) {
                PunManager.instance.UpdateTumbleLaunchRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeMapPlayerCount")) {
                PunManager.instance.UpdateMapPlayerCountRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeSpeed")) {
                PunManager.instance.UpdateSprintSpeedRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeStrength")) {
                PunManager.instance.UpdateGrabStrengthRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeRange")) {
                PunManager.instance.UpdateGrabRangeRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeThrow")) {
                PunManager.instance.UpdateThrowStrengthRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeCrouchRest")) {
                PunManager.instance.UpdateCrouchRestRightAway(steamId, level);
            } else if (key.Equals("playerUpgradeTumbleWings")) {
                PunManager.instance.UpdateTumbleWingsRightAway(steamId, level);
            }
        }

        public static int getUpgradeLevel(string type, PlayerAvatar avatar) {
            StatsManager statsManager = StatsManager.instance;
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);

            if (statsManager != null) {
                if (statsManager.dictionaryOfDictionaries.ContainsKey(type)) {
                    Dictionary<string, int> upgradeType = statsManager.dictionaryOfDictionaries[type];
                    if (upgradeType != null && upgradeType.ContainsKey(playerSteamId)) {
                        return upgradeType[playerSteamId];
                    }
                }
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

        public static void returnToTruck(PlayerAvatar avatar) {
            teleport(avatar, TruckSafetySpawnPoint.instance.transform.position, TruckSafetySpawnPoint.instance.transform.rotation);
        }

        private static void teleport(PlayerAvatar from, PlayerAvatar to) {
            UnityEngine.Vector3 pos = to.transform.position;
            UnityEngine.Quaternion rot = to.transform.rotation;
            if (to.deadSet) {
                pos = to.playerDeathHead.physGrabObject.transform.position;
                rot = to.playerDeathHead.physGrabObject.transform.rotation;
            }
            teleport(from, pos, rot);
        }

        private static void teleport(PlayerAvatar from, UnityEngine.Vector3 pos, UnityEngine.Quaternion rot) {
            if (!SemiFunc.IsMultiplayer()) {
                from.Spawn(pos, rot);
            } else if (from.photonView != null) {
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
            //NetworkUtil.SendCommandSteamIDString("KickPlayer", avatar.steamID, "", ReceiverGroup.Others);
            NetworkManager.instance.KickPlayer(avatar);
        }

        public static void BanPlayer(PlayerAvatar avatar) {
            // no banning host
            if (avatar == PlayerAvatar.instance) {
                return;
            }

            //avatar.photonView.RPC("OutroStartRPC", RpcTarget.All, new object[] { });
            RepoAdminMenu.mls.LogInfo("Sending ban to: " + SemiFunc.PlayerGetName(avatar));
            //NetworkUtil.SendCommandSteamIDString("KickPlayer", avatar.steamID, "", ReceiverGroup.Others);
            NetworkManager.instance.BanPlayer(avatar);
        }
    }
}
