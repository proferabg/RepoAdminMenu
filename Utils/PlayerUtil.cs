using Photon.Pun;
using System;

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

        public static void teleport(PlayerAvatar from, PlayerAvatar to) {
            UnityEngine.Vector3 pos = to.transform.position;
            UnityEngine.Quaternion rot = to.transform.rotation;
            if (to.deadSet) {
                pos = to.playerDeathHead.physGrabObject.transform.position;
                rot = to.playerDeathHead.physGrabObject.transform.rotation;
            }
            teleport(from, pos, rot);
        }

        private static void teleport(PlayerAvatar from, UnityEngine.Vector3 pos, UnityEngine.Quaternion rot) {
            if (from.deadSet) {
                from.playerDeathHead.physGrabObject.Teleport(pos, rot);
            } else {
                from.Spawn(pos, rot);
            }
        }

        public static void KickPlayer(PlayerAvatar avatar) {
            // no kicking host
            if (avatar == PlayerAvatar.instance) {
                return;
            }

            string name = SemiFunc.PlayerGetName(avatar);

            if (avatar.photonView == null || avatar.photonView.Owner == null) {
                RepoAdminMenu.mls.LogInfo("Photon Player is null for: " + name);
                return;
            }

            RepoAdminMenu.mls.LogInfo("Sending kick to: " + name);
            NetworkManager.instance.KickPlayer(avatar.photonView.Owner);
        }

        public static void BanPlayer(PlayerAvatar avatar) {
            // no banning host
            if (avatar == PlayerAvatar.instance) {
                return;
            }

            string name = SemiFunc.PlayerGetName(avatar);

            if (avatar.photonView == null || avatar.photonView.Owner == null) {
                RepoAdminMenu.mls.LogInfo("Photon Player is null for: " + name);
                return;
            }

            RepoAdminMenu.mls.LogInfo("Sending ban to: " + SemiFunc.PlayerGetName(avatar));
            NetworkManager.instance.BanPlayer(avatar.photonView.Owner);
        }
    }
}
