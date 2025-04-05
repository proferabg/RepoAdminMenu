using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(EnemyDirector))]
    internal class EnemyDirectorPatch {


        [HarmonyPatch("SetInvestigate")]
        [HarmonyPrefix]
        private static bool SetInvestigate_Prefix(EnemyDirector __instance) {
            return Settings.deafEnemies;
        }

    }
}
