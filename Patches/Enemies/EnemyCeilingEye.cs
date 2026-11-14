using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyCeilingEyePatches {

        [HarmonyPatch(typeof(EnemyCeilingEye), nameof(EnemyCeilingEye.Update))] // Peeper
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___targetPlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___targetPlayer, ___enemy);
        }
    }
}
