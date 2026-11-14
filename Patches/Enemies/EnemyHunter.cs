using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyHunterPatches {

        [HarmonyPatch(typeof(EnemyHunter), nameof(EnemyHunter.Update))] // Huntsman
        [HarmonyPrefix]
        private static void Update_Prefix(EnemyHunter __instance) {
            EnemyRigidbody rigidbody = __instance.enemy?.Rigidbody;
            if (rigidbody == null)
                return;

            if (EnemyNoTargetHelpers.IsNoTarget(rigidbody.onTouchPlayerAvatar)) {
                rigidbody.onTouchPlayerAvatar = null;
                __instance.UpdateState(EnemyHunter.State.Roam);
            }

            if (EnemyNoTargetHelpers.IsNoTarget(rigidbody.onGrabbedPlayerAvatar)) {
                rigidbody.onGrabbedPlayerAvatar = null;
                __instance.UpdateState(EnemyHunter.State.Roam);
            }
        }

        [HarmonyPatch(typeof(EnemyHunter), nameof(EnemyHunter.UpdateState))] // Huntsman
        [HarmonyPrefix]
        private static bool UpdateState_Prefix(EnemyHunter __instance, EnemyHunter.State _state) {
            if (_state != EnemyHunter.State.Aim)
                return true;

            foreach (PlayerAvatar player in SemiFunc.PlayerGetAll()) {
                if (EnemyNoTargetHelpers.IsNoTarget(player) &&
                    __instance.investigatePointTransform != null &&
                    player.PlayerVisionTarget?.VisionTransform != null &&
                    __instance.investigatePointTransform == player.PlayerVisionTarget.VisionTransform) {
                    __instance.currentState = EnemyHunter.State.Roam;
                    return false;
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(EnemyHunter), nameof(EnemyHunter.OnTouchPlayer))] // Huntsman
        [HarmonyPrefix]
        private static bool OnTouchPlayer_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onTouchPlayerAvatar);
        }

        [HarmonyPatch(typeof(EnemyHunter), nameof(EnemyHunter.OnTouchPlayerGrabbedObject))] // Huntsman
        [HarmonyPrefix]
        private static bool OnTouchPlayerGrabbedObject_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onTouchPlayerGrabbedObjectAvatar);
        }
    }
}
