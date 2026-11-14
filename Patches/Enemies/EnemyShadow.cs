using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyShadowPatches {

        [HarmonyPatch(typeof(EnemyShadow), nameof(EnemyShadow.Update))] // Loom
        [HarmonyPrefix]
        private static void Update_Prefix(EnemyShadow __instance, ref PlayerAvatar ___playerTarget) {
            if (!EnemyNoTargetHelpers.IsNoTarget(___playerTarget))
                return;

            ___playerTarget = null;
            __instance.UpdateState(EnemyShadow.State.ChooseTarget);
        }

        [HarmonyPatch(typeof(EnemyShadow), nameof(EnemyShadow.BendLogic))] // Loom
        [HarmonyPrefix]
        private static bool BendLogic_Prefix(ref PlayerAvatar ___playerTarget) {
            if (EnemyNoTargetHelpers.HasAllowedTarget(___playerTarget))
                return true;

            ___playerTarget = null;
            return false;
        }

        [HarmonyPatch(typeof(EnemyShadow), nameof(EnemyShadow.OnTouchPlayer))] // Loom
        [HarmonyPrefix]
        private static bool OnTouchPlayer_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onTouchPlayerAvatar);
        }

        [HarmonyPatch(typeof(EnemyShadow), nameof(EnemyShadow.OnTouchPlayerGrabbedObject))] // Loom
        [HarmonyPrefix]
        private static bool OnTouchPlayerGrabbedObject_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onTouchPlayerGrabbedObjectAvatar);
        }
    }
}
