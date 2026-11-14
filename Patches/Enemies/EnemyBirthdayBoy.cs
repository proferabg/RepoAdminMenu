using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyBirthdayBoyPatches {

        [HarmonyPatch(typeof(EnemyBirthdayBoy), nameof(EnemyBirthdayBoy.Update))] // Birthday Boy
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }
    }
}
