using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyParent))]
    internal class EnemyParentPatch {

        public static bool spawning = false;

        [HarmonyPatch("Despawn")]
        [HarmonyPrefix]
        private static bool Despawn_Prefix(float ___SpawnedTimeMin, float ___SpawnedTimeMax, ref float ___SpawnedTimer) {
            if (spawning) {
                spawning = false;
                ___SpawnedTimer = UnityEngine.Random.Range(___SpawnedTimeMin, ___SpawnedTimeMax);
                return false;
            }
            return true;
        }

    }
}
