using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(Trap))]
    internal class TrapPatch {


        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(ref bool ___trapStart, ref bool ___trapActive) {
            if (Settings.instance.noTraps) {
                ___trapActive = false;
                ___trapStart = false;
            }
        }
    }
}
