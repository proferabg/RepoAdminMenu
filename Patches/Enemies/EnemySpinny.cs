using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemySpinnyPatches {

        [HarmonyPatch(typeof(EnemySpinny), nameof(EnemySpinny.Update))] // Gambit
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }
    }
}
