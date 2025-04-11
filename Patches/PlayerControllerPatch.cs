using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerController))]
    internal class PlayerControllerPatch {

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_Prefix(PlayerController __instance, PlayerAvatar ___playerAvatarScript) {
            if (___playerAvatarScript != null){
                if (Settings.isInfiniteStamina(___playerAvatarScript)) {
                    __instance.EnergyCurrent = __instance.EnergyStart;
                }

                __instance.DebugNoTumble = Settings.isNoTumble(___playerAvatarScript);
            }
        }
    }
}
