using HarmonyLib;
using RepoAdminMenu.Utils;
using System;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(PlayerHealth))]
    internal class PlayerHealthPatch {

        [HarmonyPatch("Hurt")]
        [HarmonyPrefix]
        private static bool Hurt_Prefix(ref int damage, PlayerAvatar ___playerAvatar, int ___health) {
            if (PlayerUtil.isGod(___playerAvatar)) {
                return false;
            }

            if (PlayerUtil.isNoDeath(___playerAvatar)) {
                damage = Math.Min(damage, ___health - 1);
                if (damage == 0) {
                    return false;
                }
            }

            return true;
        }

    }
}
