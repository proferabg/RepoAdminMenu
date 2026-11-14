using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyRobePatches {

        [HarmonyPatch(typeof(EnemyRobe), nameof(EnemyRobe.Update))] // Robe
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___targetPlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___targetPlayer, ___enemy);
        }

        [HarmonyPatch(typeof(EnemyRobe), nameof(EnemyRobe.ChaseTimer))] // Robe
        [HarmonyPrefix]
        private static bool ChaseTimer_Prefix(ref PlayerAvatar ___targetPlayer) {
            if (EnemyNoTargetHelpers.HasAllowedTarget(___targetPlayer))
                return true;

            ___targetPlayer = null;
            return false;
        }
    }
}
