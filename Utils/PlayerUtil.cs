using Photon.Pun;
using System;
using RepoAdminMenu.Utils;

namespace RepoAdminMenu.Utils {
    internal class PlayerUtil {

        private DateTime lastSync = DateTime.Now;

        public static void killPlayer(PlayerAvatar avatar) {
            avatar.PlayerDeath(-1);
            RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Killed!");
        }

        public static void revivePlayer(PlayerAvatar avatar) {
            if (ReflectionUtil.PlayerAvatar_GetDeadSet(avatar) && ReflectionUtil.PlayerAvatar_GetDeathHead(avatar) != null) {
                PlayerDeathHead playerDeathHead = ReflectionUtil.PlayerAvatar_GetDeathHead(avatar);
                ReflectionUtil.PlayerDeathHead_SetInExtractionPoint(playerDeathHead, true);
                avatar.Revive();
                ReflectionUtil.PlayerDeathHead_SetInExtractionPoint(playerDeathHead, false);
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Revived!");
            } else {
                RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " is not dead. Cannot be revived!");
            }
        }

        public static void healPlayer(PlayerAvatar avatar) {
            PlayerHealth health = avatar.playerHealth;
            avatar.playerHealth.HealOther(ReflectionUtil.PlayerHealth_GetMaxHealth(health), true);
            RepoAdminMenu.mls.LogInfo(SemiFunc.PlayerGetName(avatar) + " Healed!");
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
            if (ReflectionUtil.PlayerAvatar_GetDeadSet(to)) {
                pos = ReflectionUtil.GetFieldValue<PhysGrabObject>(ReflectionUtil.PlayerAvatar_GetDeathHead(to), "physGrabObject").transform.position;
                rot = ReflectionUtil.GetFieldValue<PhysGrabObject>(ReflectionUtil.PlayerAvatar_GetDeathHead(to), "physGrabObject").transform.rotation;
            }
            teleport(from, pos, rot);
        }

        private static void teleport(PlayerAvatar from, UnityEngine.Vector3 pos, UnityEngine.Quaternion rot) {
            if (!SemiFunc.IsMultiplayer()) {
                from.Spawn(pos, rot);
            } else if (from.photonView != null) {
                if (ReflectionUtil.PlayerAvatar_GetDeadSet(from)) {
                    ReflectionUtil.GetFieldValue<PhysGrabObject>(ReflectionUtil.PlayerAvatar_GetDeathHead(from), "physGrabObject").Teleport(pos, rot);
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
