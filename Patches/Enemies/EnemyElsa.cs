using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyElsaPatches {

        [HarmonyPatch(typeof(EnemyElsa), nameof(EnemyElsa.Update))] // Elsa
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }

        [HarmonyPatch(typeof(EnemyElsa), nameof(EnemyElsa.OnHurt))] // Elsa
        [HarmonyPrefix]
        private static bool OnHurt_Prefix(ref PlayerAvatar ___playerTarget) {
            if (EnemyNoTargetHelpers.HasAllowedTarget(___playerTarget))
                return true;

            ___playerTarget = null;
            return false;
        }
    }
}
