using HarmonyLib;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyDuckPatches {

        [HarmonyPatch(typeof(EnemyDuck), nameof(EnemyDuck.Update))] // Apex Predator
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }

        [HarmonyPatch(typeof(EnemyDuck), nameof(EnemyDuck.OnGrabbed))] // Apex Predator
        [HarmonyPrefix]
        private static bool OnGrabbed_Prefix(EnemyDuck __instance, Enemy ___enemy) {
            PlayerAvatar grabbingPlayer = ___enemy?.Rigidbody?.onGrabbedPlayerAvatar;
            if (!Settings.instance.friendlyDuck && EnemyNoTargetHelpers.Allows(grabbingPlayer))
                return true;

            __instance.currentState = EnemyDuck.State.Roam;
            return false;
        }

        [HarmonyPatch(typeof(EnemyDuck), nameof(EnemyDuck.UpdateState))] // Apex Predator
        [HarmonyPrefix]
        private static void UpdateState_Prefix(ref EnemyDuck.State _state) {
            if (Settings.instance.friendlyDuck && _state == EnemyDuck.State.AttackStart)
                _state = EnemyDuck.State.Roam;
        }
    }
}
