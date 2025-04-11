using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(ExtractionPoint))]
    internal class ExtractionPointPatch {

        private static int previousMoney = -1;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(ExtractionPoint __instance, bool ___isShop) {
            if (___isShop) {
                if (Settings.instance.infiniteMoney) {
                    if (previousMoney < 0) {
                        previousMoney = StatsManager.instance.runStats["currency"];
                    }
                    StatsManager.instance.runStats["currency"] = 9999;
                } else {
                    if (previousMoney >= 0) {
                        StatsManager.instance.runStats["currency"] = previousMoney;
                    }
                    previousMoney = -1;
                }
            }
        }
    }
}
