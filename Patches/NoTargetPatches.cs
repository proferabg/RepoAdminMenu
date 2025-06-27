using HarmonyLib;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal class NoTargetPatches {

        [HarmonyPatch(typeof(Enemy), "SetChaseTarget")]
        [HarmonyPrefix]
        private static bool Enemy_SetChaseTarget_Prefix(PlayerAvatar playerAvatar) {
            return !Settings.isNoTarget(playerAvatar);
        }

        [HarmonyPatch(typeof(EnemyOnScreen), "GetOnScreen")]
        [HarmonyPrefix]
        private static bool EnemyOnScreen_GetOnScreen_Prefix(PlayerAvatar _playerAvatar) {
            return !Settings.isNoTarget(_playerAvatar);
        }

        [HarmonyPatch(typeof(Enemy), "Update")]
        [HarmonyPrefix]
        private static void Enemy_Update_Prefix(Enemy __instance, ref PlayerAvatar ___TargetPlayerAvatar) {
            if (___TargetPlayerAvatar != null && Settings.isNoTarget(___TargetPlayerAvatar)) {
                ___TargetPlayerAvatar = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyAnimal), "Update")]
        [HarmonyPrefix]
        private static void EnemyAnimal_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyBangDirector), "Update")]
        [HarmonyPrefix]
        private static void EnemyBangDirector_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyBeamer), "Update")]
        [HarmonyPrefix]
        private static void EnemyBeamer_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyBowtie), "Update")]
        [HarmonyPrefix]
        private static void EnemyBowtie_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyCeilingEye), "Update")]
        [HarmonyPrefix]
        private static void EnemyCeilingEye_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyFloater), "Update")]
        [HarmonyPrefix]
        private static void EnemyFloater_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyGnomeDirector), "Update")]
        [HarmonyPrefix]
        private static void EnemyGnomeDirector_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyHidden), "Update")]
        [HarmonyPrefix]
        private static void EnemyHidden_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyRobe), "Update")]
        [HarmonyPrefix]
        private static void EnemyRobe_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyRunner), "Update")]
        [HarmonyPrefix]
        private static void EnemyRunner_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemySlowMouth), "Update")]
        [HarmonyPrefix]
        private static void EnemySlowMouth_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemySlowWalker), "Update")]
        [HarmonyPrefix]
        private static void EnemySlowWalker_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyThinMan), "Update")]
        [HarmonyPrefix]
        private static void EnemyThinMan_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyTumbler), "Update")]
        [HarmonyPrefix]
        private static void EnemyTumbler_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyUpscream), "Update")]
        [HarmonyPrefix]
        private static void EnemyUpscream_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyValuableThrower), "Update")]
        [HarmonyPrefix]
        private static void EnemyValuableThrower_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyDuck), "Update")]
        [HarmonyPrefix]
        private static void EnemyDuck_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyDuck), "OnGrabbed")]
        [HarmonyPrefix]
        private static bool EnemyDuck_OnGrabbed_Prefix(EnemyDuck __instance) {
            if (Settings.instance.friendlyDuck) {
                __instance.currentState = EnemyDuck.State.Roam;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(EnemyDuck), "UpdateState")]
        [HarmonyPrefix]
        private static void EnemyDuck_UpdateState_Prefix(ref EnemyDuck.State _state) {
            if (Settings.instance.friendlyDuck && _state == EnemyDuck.State.AttackStart) {
                _state = EnemyDuck.State.Roam;
            }
        }
    }
}
