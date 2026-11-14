using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyFloaterPatches {

        [HarmonyPatch(typeof(EnemyFloater), nameof(EnemyFloater.Update))] // Mentalist
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___targetPlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___targetPlayer, ___enemy);
        }
    }
}
