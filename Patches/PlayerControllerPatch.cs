using HarmonyLib;
using RepoAdminMenu.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerController))]
    internal class PlayerControllerPatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(PlayerController __instance, PlayerAvatar ___playerAvatarScript) {
            if (___playerAvatarScript != null){
                if (PlayerUtil.isInfiniteStamina(___playerAvatarScript)) {
                    __instance.EnergyCurrent = __instance.EnergyStart;
                }

                __instance.DebugNoTumble = PlayerUtil.isNoTumble(___playerAvatarScript);
            }
        }
    }
}
