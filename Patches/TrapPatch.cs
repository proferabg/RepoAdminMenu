using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(Trap))]
    internal class TrapPatch {


        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(ref bool ___trapStart, ref bool ___trapActive) {
            if (Settings.noTraps) {
                ___trapActive = false;
                ___trapStart = false;
            }
        }
    }
}
