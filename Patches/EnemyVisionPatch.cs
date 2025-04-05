using HarmonyLib;
using RepoAdminMenu.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyVision))]
    internal class EnemyVisionPatch {


        [HarmonyPatch("VisionTrigger")]
        [HarmonyPrefix]
        private static bool VisionTrigger_Prefix(int playerID, PlayerAvatar player, EnemyVision __instance) {
            if(Settings.blindEnemies || PlayerUtil.isNoTarget(player)) {
                __instance.VisionTriggered[playerID] = false;
                __instance.VisionsTriggered[playerID] = 0;
                return false;
            }
            return true;
        }
    }
}
