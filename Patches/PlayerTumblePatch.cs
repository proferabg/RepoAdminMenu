using HarmonyLib;
using RepoAdminMenu.Utils;
using System;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerTumble))]
    internal class PlayerTumblePatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(PlayerTumble __instance, PlayerAvatar ___playerAvatar) {
            if (__instance != null && PlayerUtil.isForceTumble(___playerAvatar)) {
                long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (PlayerUtil.getLastForceTumble(___playerAvatar) < (currentMillis - 500)) {
                    __instance.TumbleRequest(true, false);
                    PlayerUtil.setLastForceTumble(___playerAvatar, currentMillis);
                }
            }
        }

    }
}
