using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemySlowMouthPatches {

        [HarmonyPatch(typeof(EnemySlowMouth), nameof(EnemySlowMouth.Update))] // Spewer
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }
    }
}
