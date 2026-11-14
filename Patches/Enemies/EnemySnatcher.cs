using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemySnatcherPatches {

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.Update))] // Pony
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget, ref PlayerAvatar ___ceilingScarePlayer) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
            if (EnemyNoTargetHelpers.IsNoTarget(___ceilingScarePlayer))
                ___ceilingScarePlayer = null;
        }

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.SetPlayerTarget))] // Pony
        [HarmonyPrefix]
        private static bool SetPlayerTarget_Prefix(PlayerAvatar _player) {
            return EnemyNoTargetHelpers.Allows(_player);
        }

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.PlayerTargetAssign))] // Pony
        [HarmonyPrefix]
        private static bool PlayerTargetAssign_Prefix(PlayerAvatar _player) {
            return EnemyNoTargetHelpers.Allows(_player);
        }

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.CeilingScareLanding))] // Pony
        [HarmonyPrefix]
        private static bool CeilingScareLanding_Prefix(PlayerAvatar _player, ref bool __result) {
            if (EnemyNoTargetHelpers.Allows(_player))
                return true;

            __result = false;
            return false;
        }

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.OnGrabbed))] // Pony
        [HarmonyPrefix]
        private static bool OnGrabbed_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onGrabbedPlayerAvatar);
        }

        [HarmonyPatch(typeof(EnemySnatcher), nameof(EnemySnatcher.OnTouchPlayer))] // Pony
        [HarmonyPrefix]
        private static bool OnTouchPlayer_Prefix(Enemy ___enemy) {
            return EnemyNoTargetHelpers.Allows(___enemy?.Rigidbody?.onTouchPlayerAvatar);
        }

        [HarmonyPatch(typeof(SemiMounting), nameof(SemiMounting.SetSeatGrabber))] // Pony
        [HarmonyPrefix]
        private static bool SetSeatGrabber_Prefix(SemiMounting __instance, PlayerAvatar _playerAvatar) {
            return !IsPonyMount(__instance) || EnemyNoTargetHelpers.Allows(_playerAvatar);
        }

        [HarmonyPatch(typeof(SemiMounting), nameof(SemiMounting.SetSeatGrabberLocal))] // Pony
        [HarmonyPrefix]
        private static bool SetSeatGrabberLocal_Prefix(SemiMounting __instance, PlayerAvatar _playerAvatar) {
            return !IsPonyMount(__instance) || EnemyNoTargetHelpers.Allows(_playerAvatar);
        }

        private static bool IsPonyMount(SemiMounting mounting) {
            Enemy enemy = mounting?.enemyRigidbody?.enemy;
            if (enemy == null)
                return false;

            return (enemy.EnemyParent != null && enemy.EnemyParent.GetComponentInChildren<EnemySnatcher>() != null) ||
                enemy.GetComponent<EnemySnatcher>() != null ||
                enemy.GetComponentInParent<EnemySnatcher>() != null ||
                enemy.GetComponentInChildren<EnemySnatcher>() != null;
        }
    }
}
