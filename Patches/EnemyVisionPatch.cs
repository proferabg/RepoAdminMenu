using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyVision))]
    internal class EnemyVisionPatch {


        [HarmonyPatch("VisionTrigger")]
        [HarmonyPrefix]
        private static bool VisionTrigger_Prefix(int playerID, PlayerAvatar player, EnemyVision __instance) {
            if(Settings.instance.blindEnemies || Settings.isNoTarget(player)) {
                __instance.VisionTriggered[playerID] = false;
                __instance.VisionsTriggered[playerID] = 0;
                return false;
            }
            return true;
        }
    }
}
