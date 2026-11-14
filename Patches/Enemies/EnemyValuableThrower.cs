using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyValuableThrowerPatches {

        [HarmonyPatch(typeof(EnemyValuableThrower), nameof(EnemyValuableThrower.Update))] // Rugrat
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }
    }
}
