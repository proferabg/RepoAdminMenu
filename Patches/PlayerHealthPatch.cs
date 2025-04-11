using HarmonyLib;
using System;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerHealth))]
    internal class PlayerHealthPatch {

        [HarmonyPatch("Hurt")]
        [HarmonyPrefix]
        private static bool Hurt_Prefix(ref int damage, PlayerAvatar ___playerAvatar, int ___health) {
            if (Settings.isGod(___playerAvatar)) {
                return false;
            }

            if (Settings.isNoDeath(___playerAvatar)) {
                damage = Math.Min(damage, ___health - 1);
                if (damage == 0) {
                    return false;
                }
            }

            return true;
        }

    }
}
