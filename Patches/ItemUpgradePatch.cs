using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(ItemUpgrade))]
    internal class ItemUpgradePatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(ItemToggle ___itemToggle) {
            if (Settings.useShopUpgrades) {
                ___itemToggle.enabled = true;
            }

        }
    }
}
