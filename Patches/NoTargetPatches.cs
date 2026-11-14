using HarmonyLib;
using RepoAdminMenu.Patches.Enemies;
using System;
using System.Collections.Generic;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal static class NoTargetPatches {

        [ThreadStatic]
        private static int noTargetPlayerNoiseDepth;

        [HarmonyPatch(typeof(SemiFunc), nameof(SemiFunc.PlayerGetAllPlayerAvatarWithinRange))] // All enemies
        [HarmonyPostfix]
        private static void PlayerGetAllPlayerAvatarWithinRange_Postfix(ref List<PlayerAvatar> __result) {
            __result = __result.FindAll(EnemyNoTargetHelpers.Allows);
        }

        [HarmonyPatch(typeof(Enemy), nameof(Enemy.SetChaseTarget))] // All enemies
        [HarmonyPrefix]
        private static bool SetChaseTarget_Prefix(PlayerAvatar playerAvatar) {
            return EnemyNoTargetHelpers.Allows(playerAvatar);
        }

        [HarmonyPatch(typeof(EnemyOnScreen), nameof(EnemyOnScreen.GetOnScreen))] // All enemies
        [HarmonyPrefix]
        private static bool GetOnScreen_Prefix(PlayerAvatar _playerAvatar) {
            return EnemyNoTargetHelpers.Allows(_playerAvatar);
        }

        [HarmonyPatch(typeof(Enemy), nameof(Enemy.Update))] // All enemies
        [HarmonyPrefix]
        private static void Enemy_Update_Prefix(Enemy __instance, ref PlayerAvatar ___TargetPlayerAvatar) {
            EnemyNoTargetHelpers.ClearTarget(ref ___TargetPlayerAvatar, __instance);
        }

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.Footstep))] // Player footstep noise
        [HarmonyPrefix]
        private static void PlayerAvatar_Footstep_Prefix(PlayerAvatar __instance, out bool __state) {
            __state = BeginPlayerNoise(__instance);
        }

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.Footstep))] // Player footstep noise
        [HarmonyPostfix]
        private static void PlayerAvatar_Footstep_Postfix(bool __state) {
            EndPlayerNoise(__state);
        }

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.LandRPC))] // Player landing noise
        [HarmonyPrefix]
        private static void PlayerAvatar_LandRPC_Prefix(PlayerAvatar __instance, out bool __state) {
            __state = BeginPlayerNoise(__instance);
        }

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.LandRPC))] // Player landing noise
        [HarmonyPostfix]
        private static void PlayerAvatar_LandRPC_Postfix(bool __state) {
            EndPlayerNoise(__state);
        }

        [HarmonyPatch(typeof(PlayerVoiceChat), nameof(PlayerVoiceChat.Update))] // Player voice noise
        [HarmonyPrefix]
        private static void PlayerVoiceChat_Update_Prefix(PlayerAvatar ___playerAvatar, out bool __state) {
            __state = BeginPlayerNoise(___playerAvatar);
        }

        [HarmonyPatch(typeof(PlayerVoiceChat), nameof(PlayerVoiceChat.Update))] // Player voice noise
        [HarmonyPostfix]
        private static void PlayerVoiceChat_Update_Postfix(bool __state) {
            EndPlayerNoise(__state);
        }

        [HarmonyPatch(typeof(EnemyDirector), nameof(EnemyDirector.SetInvestigate))] // All enemy sound investigations
        [HarmonyPrefix]
        private static bool EnemyDirector_SetInvestigate_Prefix() {
            return noTargetPlayerNoiseDepth == 0;
        }

        private static bool BeginPlayerNoise(PlayerAvatar player) {
            bool suppressInvestigation = EnemyNoTargetHelpers.IsNoTarget(player);
            if (suppressInvestigation)
                noTargetPlayerNoiseDepth++;
            return suppressInvestigation;
        }

        private static void EndPlayerNoise(bool suppressInvestigation) {
            if (suppressInvestigation && noTargetPlayerNoiseDepth > 0)
                noTargetPlayerNoiseDepth--;
        }
    }
}
