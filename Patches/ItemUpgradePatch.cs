using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(ItemUpgrade))]
    internal class ItemUpgradePatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(ItemToggle ___itemToggle) {
            if (Settings.instance.useShopUpgrades) {
                ___itemToggle.enabled = true;
            }

        }
    }
}
