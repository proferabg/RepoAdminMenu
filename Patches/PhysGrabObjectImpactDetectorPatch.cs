using HarmonyLib;

namespace RepoAdminMenu.Patches { 

    [HarmonyPatch(typeof(PhysGrabObjectImpactDetector))]
    internal class PhysGrabObjectImpactDetectorPatch {


        [HarmonyPatch("Break")]
        [HarmonyPrefix]
        private static bool Break_Prefix(bool ___isEnemy) {
            return (!Settings.instance.noBreak || ___isEnemy);
        }

    }
}
