using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyAnimalPatches {

        [HarmonyPatch(typeof(EnemyAnimal), nameof(EnemyAnimal.Update))] // Animal
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }
    }
}
