using HarmonyLib;
using System;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerTumble))]
    internal class PlayerTumblePatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(PlayerTumble __instance, PlayerAvatar ___playerAvatar) {
            if (__instance != null && ___playerAvatar != null && Settings.isForceTumble(___playerAvatar) && SemiFunc.IsMasterClient()) {
                long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (Settings.getLastForceTumble(___playerAvatar) < (currentMillis - 500)) {
                    __instance.TumbleRequest(true, false);
                    Settings.setLastForceTumble(___playerAvatar, currentMillis);
                }
            }
        }

    }
}
