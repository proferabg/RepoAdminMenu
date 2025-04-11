using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(ItemBattery))]
    internal class ItemBatteryPatch {

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void Update_Postfix(ItemBattery __instance, ref int ___batteryLifeInt) {
            if (Settings.instance.noBatteryDrain) {
                __instance.batteryLife = 100f;
                ___batteryLifeInt = 6;
            }
        }
    }
}
