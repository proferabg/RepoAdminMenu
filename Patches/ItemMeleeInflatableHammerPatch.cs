using HarmonyLib;
using Photon.Pun;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(ItemMeleeInflatableHammer))]
    internal class ItemMeleeInflatableHammerPatch {

        [HarmonyPatch("OnHit")]
        [HarmonyPrefix]
        private static bool OnHit_Prefix(ItemMeleeInflatableHammer __instance, PhotonView ___photonView) {
            if (Settings.instance.boomhammer) {
                if (SemiFunc.IsMasterClientOrSingleplayer()) {
                    __instance.ExplosionRPC();
                    return false;
                }
                if (SemiFunc.IsMasterClient()) {
                    ___photonView.RPC("ExplosionRPC", RpcTarget.Others, new object[] {});
                    return false;
                }
            }
            return true;

        }

    }
}
