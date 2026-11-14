using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyTricyclePatches {

        [HarmonyPatch(typeof(EnemyTricycle), nameof(EnemyTricycle.Update))] // Bella
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }

        [HarmonyPatch(typeof(EnemyTricycle), nameof(EnemyTricycle.SetState))] // Bella
        [HarmonyPrefix]
        private static void SetState_Prefix(ref PlayerAvatar ___playerTarget, ref PlayerAvatar ___isBlockedByPlayerAvatar, ref EnemyTricycle.State newState) {
            if (EnemyNoTargetHelpers.IsNoTarget(___playerTarget)) {
                ___playerTarget = null;
                newState = EnemyTricycle.State.Roam;
            }
            if (EnemyNoTargetHelpers.IsNoTarget(___isBlockedByPlayerAvatar)) {
                ___isBlockedByPlayerAvatar = null;
                newState = EnemyTricycle.State.Roam;
            }
        }

        [HarmonyPatch(typeof(EnemyTricycle), nameof(EnemyTricycle.StateAttack))] // Bella
        [HarmonyPrefix]
        private static bool StateAttack_Prefix(EnemyTricycle __instance, ref PlayerAvatar ___playerTarget) {
            if (EnemyNoTargetHelpers.HasAllowedTarget(___playerTarget))
                return true;

            ___playerTarget = null;
            __instance.SetState(EnemyTricycle.State.AttackOutro);
            return false;
        }

        [HarmonyPatch(typeof(EnemyTricycle), nameof(EnemyTricycle.OnGrabbed))] // Bella
        [HarmonyPrefix]
        private static bool OnGrabbed_Prefix(
            EnemyTricycle __instance,
            Enemy ___enemy,
            ref PlayerAvatar ___playerTarget,
            ref PlayerAvatar ___nearbyPlayer,
            ref PlayerAvatar ___isBlockedByPlayerAvatar,
            ref bool ___isBlockedByPlayer
        ) {
            PlayerAvatar grabbingPlayer = ___enemy?.Rigidbody?.onGrabbedPlayerAvatar;
            if (EnemyNoTargetHelpers.Allows(grabbingPlayer))
                return true;

            ___playerTarget = null;
            ___nearbyPlayer = null;
            ___isBlockedByPlayerAvatar = null;
            ___isBlockedByPlayer = false;
            __instance.SetState(EnemyTricycle.State.Roam);
            return false;
        }
    }
}
