using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyDirector))]
    internal class EnemyDirectorPatch {


        [HarmonyPatch("SetInvestigate")]
        [HarmonyPrefix]
        private static bool SetInvestigate_Prefix(EnemyDirector __instance) {
            return Settings.instance.deafEnemies;
        }

    }
}
