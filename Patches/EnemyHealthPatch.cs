using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyHealth))]
    internal class EnemyHealthPatch {

        [HarmonyPatch("Hurt")]
        [HarmonyPrefix]
        private static void Hurt_Prefix(ref int _damage, int ___healthCurrent) {
            if (Settings.instance.weakEnemies) {
                _damage = ___healthCurrent;
                // sanity check
                if (___healthCurrent < 1)
                    _damage = 100;
            }
        }

    }
}
