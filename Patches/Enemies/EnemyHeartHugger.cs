using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyHeartHuggerPatches {

        [HarmonyPatch(typeof(EnemyHeartHugger), nameof(EnemyHeartHugger.Update))] // Heart Hugger
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___currentTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___currentTarget, ___enemy);
        }
    }
}
