using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyRunnerPatches {

        [HarmonyPatch(typeof(EnemyRunner), nameof(EnemyRunner.Update))] // Reaper
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___targetPlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___targetPlayer, ___enemy);
        }
    }
}
