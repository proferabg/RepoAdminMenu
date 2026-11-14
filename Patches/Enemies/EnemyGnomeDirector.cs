using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyGnomeDirectorPatches {

        [HarmonyPatch(typeof(EnemyGnomeDirector), nameof(EnemyGnomeDirector.Update))] // Gnome
        [HarmonyPrefix]
        private static void Update_Prefix(EnemyGnomeDirector __instance, ref PlayerAvatar ___playerTarget) {
            if (!EnemyNoTargetHelpers.IsNoTarget(___playerTarget))
                return;

            ___playerTarget = null;
            __instance.UpdateState(EnemyGnomeDirector.State.Idle);
        }
    }
}
