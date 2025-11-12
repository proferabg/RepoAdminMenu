using HarmonyLib;
using System.Collections.Generic;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal class NoTargetPatches {

        [HarmonyPatch(typeof(SemiFunc), "PlayerGetAllPlayerAvatarWithinRange")]
        [HarmonyPostfix]
        private static void SemiFunc_PlayerGetAllPlayerAvatarWithinRange_Postfix(ref List<PlayerAvatar> __result) {
            List<PlayerAvatar> newList = new List<PlayerAvatar>();
            foreach (PlayerAvatar avatar in __result) {
                if(!Settings.isNoTarget(avatar))
                    newList.Add(avatar);
            }
            __result = newList;
        }

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

        [HarmonyPatch(typeof(EnemyBirthdayBoy), "Update")]
        [HarmonyPrefix]
        private static void EnemyBirthdayBoy_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyBombThrower), "Update")]
        [HarmonyPrefix]
        private static void EnemyBombThrower_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
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

        [HarmonyPatch(typeof(EnemyElsa), "Update")]
        [HarmonyPrefix]
        private static void EnemyElsa_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyElsa), "OnHurt")]
        [HarmonyPrefix]
        private static bool EnemyElsa_OnHurt_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget == null || Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                return false;
            }
            return true;
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

        [HarmonyPatch(typeof(EnemyHeadGrabber), "Update")]
        [HarmonyPrefix]
        private static void EnemyHeadGrabber_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyHeartHugger), "Update")]
        [HarmonyPrefix]
        private static void EnemyHeartHugger_Update_Prefix(Enemy __instance, ref PlayerAvatar ___currentTarget) {
            if (___currentTarget != null && Settings.isNoTarget(___currentTarget)) {
                ___currentTarget = null;
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

        [HarmonyPatch(typeof(EnemyHunter), "Update")]
        [HarmonyPrefix]
        private static void EnemyHunter_Update_Prefix(EnemyHunter __instance) {
            if(__instance.enemy.Rigidbody != null) {
                EnemyRigidbody rigidbody = __instance.enemy.Rigidbody;
                if (rigidbody.onTouchPlayerAvatar != null && Settings.isNoTarget(rigidbody.onTouchPlayerAvatar)) {
                    rigidbody.onTouchPlayerAvatar = null;
                    __instance.UpdateState(EnemyHunter.State.Roam);

                }
                if(rigidbody.onGrabbedPlayerAvatar != null && Settings.isNoTarget(rigidbody.onGrabbedPlayerAvatar)) {
                    rigidbody.onGrabbedPlayerAvatar = null;
                    __instance.UpdateState(EnemyHunter.State.Roam);
                }
            }
        }

        [HarmonyPatch(typeof(EnemyHunter), "UpdateState")]
        [HarmonyPrefix]
        private static bool EnemyHunter_UpdateState_Prefix(EnemyHunter __instance, EnemyHunter.State _state) {
            if(_state == EnemyHunter.State.Aim) {
                foreach (PlayerAvatar player in SemiFunc.PlayerGetAll()) {
                    if (Settings.isNoTarget(player) && __instance.investigatePointTransform != null &&
                        player.PlayerVisionTarget != null && player.PlayerVisionTarget.VisionTransform != null &&
                        __instance.investigatePointTransform == player.PlayerVisionTarget.VisionTransform) {
                        __instance.currentState = EnemyHunter.State.Roam;
                        return false;
                    }
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(EnemyOogly), "Update")]
        [HarmonyPrefix]
        private static void EnemyOogly_Update_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer != null && Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
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

        [HarmonyPatch(typeof(EnemyRobe), "ChaseTimer")]
        [HarmonyPrefix]
        private static bool EnemyRobe_ChaseTimer_Prefix(Enemy __instance, ref PlayerAvatar ___targetPlayer) {
            if (___targetPlayer == null || Settings.isNoTarget(___targetPlayer)) {
                ___targetPlayer = null;
                return false;
            }
            return true;
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

        [HarmonyPatch(typeof(EnemyShadow), "Update")]
        [HarmonyPrefix]
        private static void EnemyShadow_Update_Prefix(EnemyShadow __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.UpdateState(EnemyShadow.State.ChooseTarget);
            }
        }

        [HarmonyPatch(typeof(EnemyShadow), "BendLogic")]
        [HarmonyPrefix]
        private static bool EnemyShadow_BendLogic_Prefix(EnemyShadow __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget == null || Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                return false;
            }
            return true;
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

        [HarmonyPatch(typeof(EnemySpinny), "Update")]
        [HarmonyPrefix]
        private static void EnemySpinny_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
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

        [HarmonyPatch(typeof(EnemyTick), "Update")]
        [HarmonyPrefix]
        private static void EnemyTick_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyTricycle), "Update")]
        [HarmonyPrefix]
        private static void EnemyTricycle_Update_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.CurrentState = EnemyState.Roaming;
                __instance.DisableChase(1);
            }
        }

        [HarmonyPatch(typeof(EnemyTricycle), "SetState")]
        [HarmonyPrefix]
        private static void EnemyTricycle_SetState_Prefix(Enemy __instance, ref PlayerAvatar ___playerTarget, ref PlayerAvatar ___isBlockedByPlayerAvatar, ref EnemyTricycle.State newState) {
            if (___playerTarget != null && Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                newState = EnemyTricycle.State.Roam;
            }
            if (___isBlockedByPlayerAvatar != null && Settings.isNoTarget(___isBlockedByPlayerAvatar)) {
                ___isBlockedByPlayerAvatar = null;
                newState = EnemyTricycle.State.Roam;
            }
        }

        [HarmonyPatch(typeof(EnemyTricycle), "StateAttack")]
        [HarmonyPrefix]
        private static bool EnemyTricycle_StateAttack_Prefix(EnemyTricycle __instance, ref PlayerAvatar ___playerTarget) {
            if (___playerTarget == null || Settings.isNoTarget(___playerTarget)) {
                ___playerTarget = null;
                __instance.SetState(EnemyTricycle.State.AttackOutro);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(EnemyTricycle), "OnGrabbed")]
        [HarmonyPrefix]
        private static bool EnemyTricycle_OnGrabbed_Prefix(EnemyTricycle __instance) {
            if(__instance.enemy.Rigidbody != null && __instance.enemy.Rigidbody.onGrabbedPlayerAvatar != null && 
                Settings.isNoTarget(__instance.enemy.Rigidbody.onGrabbedPlayerAvatar)) {
                return false;
            }
            return true;
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
