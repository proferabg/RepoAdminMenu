using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyTumblerPatches {

        [HarmonyPatch(typeof(EnemyTumbler), nameof(EnemyTumbler.Update))] // Chef
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___targetPlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___targetPlayer, ___enemy);
        }
    }
}
