using HarmonyLib;
using RepoAdminMenu.Patches.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal static class ShopKeeperPatches {

        private static bool ShouldIgnore(PlayerAvatar player) {
            return Settings.instance.disableShopKeeper || EnemyNoTargetHelpers.IsNoTarget(player);
        }

        private static bool IsHostileState(ShopKeeper.State state) {
            return state >= ShopKeeper.State.Flash && state <= ShopKeeper.State.SeeWarnPlayer;
        }

        private static void ForgetPlayer(ShopKeeper shopKeeper, PlayerAvatar player) {
            if (player == null)
                return;

            shopKeeper.ruckusRecords.Remove(player);
            shopKeeper.carryThrustRecords.Remove(player);
            shopKeeper.ruckusScores.Remove(player);
            shopKeeper.ruckusPunishFromScoreCache.Remove(player);
            shopKeeper.ruckusRecordKeyCache.Remove(player);
        }

        private static void ForgetIgnoredPlayers(ShopKeeper shopKeeper) {
            HashSet<PlayerAvatar> ignoredPlayers = new HashSet<PlayerAvatar>();

            foreach (PlayerAvatar player in shopKeeper.ruckusRecords.Keys) {
                if (EnemyNoTargetHelpers.IsNoTarget(player))
                    ignoredPlayers.Add(player);
            }
            foreach (PlayerAvatar player in shopKeeper.carryThrustRecords.Keys) {
                if (EnemyNoTargetHelpers.IsNoTarget(player))
                    ignoredPlayers.Add(player);
            }
            foreach (PlayerAvatar player in shopKeeper.ruckusScores.Keys) {
                if (EnemyNoTargetHelpers.IsNoTarget(player))
                    ignoredPlayers.Add(player);
            }

            foreach (PlayerAvatar player in ignoredPlayers)
                ForgetPlayer(shopKeeper, player);
        }

        private static void ClearIgnoredTargets(ShopKeeper shopKeeper) {
            if (Settings.instance.disableShopKeeper) {
                Disable(shopKeeper);
                return;
            }

            ForgetIgnoredPlayers(shopKeeper);

            bool playerIgnored = ShouldIgnore(shopKeeper.playerTarget);
            bool previousPlayerIgnored = ShouldIgnore(shopKeeper.prevPlayerTarget);
            bool itemPlayerIgnored = ShouldIgnore(shopKeeper.itemReactPlayer);

            if (playerIgnored)
                ForgetPlayer(shopKeeper, shopKeeper.playerTarget);
            if (previousPlayerIgnored)
                ForgetPlayer(shopKeeper, shopKeeper.prevPlayerTarget);
            if (itemPlayerIgnored)
                ForgetPlayer(shopKeeper, shopKeeper.itemReactPlayer);

            if (itemPlayerIgnored)
                shopKeeper.EndItemReactionSilent();

            if (playerIgnored) {
                shopKeeper.ForceClearGrabBeamOverride(false);
                shopKeeper.playerTarget = null;
                shopKeeper.ruckusTarget = false;
            }

            if (previousPlayerIgnored)
                shopKeeper.prevPlayerTarget = null;
            if (itemPlayerIgnored)
                shopKeeper.itemReactPlayer = null;

            if (itemPlayerIgnored || (playerIgnored && IsHostileState(shopKeeper.currentState)))
                shopKeeper.UpdateState(ShopKeeper.State.Observe);
        }

        private static void Disable(ShopKeeper shopKeeper) {
            shopKeeper.EndItemReactionSilent();
            shopKeeper.ForceClearGrabBeamOverride(false);

            shopKeeper.playerTarget = null;
            shopKeeper.prevPlayerTarget = null;
            shopKeeper.itemReactPlayer = null;
            shopKeeper.ruckusTarget = false;

            shopKeeper.ruckusRecords.Clear();
            shopKeeper.carryThrustRecords.Clear();
            shopKeeper.ruckusScores.Clear();
            shopKeeper.ruckusPunishFromScoreCache.Clear();
            shopKeeper.ruckusRecordKeyCache.Clear();

            shopKeeper.UpdateState(ShopKeeper.State.Sleeping);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.Update))]
        [HarmonyPrefix]
        private static bool Update_Prefix(ShopKeeper __instance) {
            if (Settings.instance.disableShopKeeper) {
                Disable(__instance);
                return false;
            }

            ClearIgnoredTargets(__instance);
            return true;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.PlayerTargetValid))]
        [HarmonyPostfix]
        private static void PlayerTargetValid_Postfix(ShopKeeper __instance, ref bool __result) {
            if (ShouldIgnore(__instance.playerTarget))
                __result = false;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.NearestPlayerAsTarget))]
        [HarmonyPostfix]
        private static void NearestPlayerAsTarget_Postfix(ShopKeeper __instance) {
            ClearIgnoredTargets(__instance);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.UpdatePlayerTargetRPC))]
        [HarmonyPostfix]
        private static void UpdatePlayerTargetRPC_Postfix(ShopKeeper __instance) {
            ClearIgnoredTargets(__instance);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.AddRuckusScore))]
        [HarmonyPrefix]
        private static bool AddRuckusScore_Prefix(PlayerAvatar _player) {
            return !ShouldIgnore(_player);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.ItemPresentationTargetValid))]
        [HarmonyPostfix]
        private static void ItemPresentationTargetValid_Postfix(PlayerAvatar _player, ref bool __result) {
            if (ShouldIgnore(_player))
                __result = false;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.ItemPresentationCanStart))]
        [HarmonyPostfix]
        private static void ItemPresentationCanStart_Postfix(PlayerAvatar _player, ref bool __result) {
            if (ShouldIgnore(_player))
                __result = false;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.TryStartItemDiscount))]
        [HarmonyPostfix]
        private static void TryStartItemDiscount_Postfix(PlayerAvatar _player, ref bool __result) {
            if (ShouldIgnore(_player))
                __result = false;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.ResolveRuckusPunishment))]
        [HarmonyPrefix]
        private static bool ResolveRuckusPunishment_Prefix(PlayerAvatar _player) {
            return !ShouldIgnore(_player);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.TriggerAngryObservation))]
        [HarmonyPrefix]
        private static bool TriggerAngryObservation_Prefix(PlayerAvatar _offender) {
            return !ShouldIgnore(_offender);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.WarnPlayer))]
        [HarmonyPrefix]
        private static bool WarnPlayer_Prefix(PlayerAvatar _player) {
            return !ShouldIgnore(_player);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.ShootAtPlayer))]
        [HarmonyPrefix]
        private static bool ShootAtPlayer_Prefix(PlayerAvatar _player) {
            return !ShouldIgnore(_player);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.ItemNoticeStart))]
        [HarmonyPrefix]
        private static bool ItemNoticeStart_Prefix(PlayerAvatar _player) {
            return !ShouldIgnore(_player);
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.WakeUpOrSleepLogic))]
        [HarmonyPrefix]
        private static bool WakeUpOrSleepLogic_Prefix() {
            return !Settings.instance.disableShopKeeper;
        }

        [HarmonyPatch(typeof(ShopKeeper), nameof(ShopKeeper.OnTriggerEnterLogic))]
        [HarmonyPrefix]
        private static bool OnTriggerEnterLogic_Prefix(Collider other) {
            if (Settings.instance.disableShopKeeper)
                return false;

            PlayerAvatar player = other.GetComponentInParent<PlayerAvatar>();
            return !EnemyNoTargetHelpers.IsNoTarget(player);
        }
    }
}
