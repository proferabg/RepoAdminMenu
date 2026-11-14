using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyBangDirectorPatches {

        [HarmonyPatch(typeof(EnemyBangDirector), nameof(EnemyBangDirector.Update))] // Bang
        [HarmonyPrefix]
        private static void Update_Prefix(EnemyBangDirector __instance, ref PlayerAvatar ___playerTarget) {
            if (!EnemyNoTargetHelpers.IsNoTarget(___playerTarget))
                return;

            ___playerTarget = null;
            __instance.UpdateState(EnemyBangDirector.State.Idle);
        }

        [HarmonyPatch(typeof(EnemyBangDirector), nameof(EnemyBangDirector.SetTarget))] // Bang
        [HarmonyPrefix]
        private static bool SetTarget_Prefix(PlayerAvatar _player) {
            return EnemyNoTargetHelpers.Allows(_player);
        }
    }
}
