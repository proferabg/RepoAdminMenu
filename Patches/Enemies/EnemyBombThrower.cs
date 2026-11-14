using HarmonyLib;
using UnityEngine;

namespace RepoAdminMenu.Patches.Enemies {

    [HarmonyPatch]
    internal static class EnemyBombThrowerPatches {

        [HarmonyPatch(typeof(EnemyBombThrower), nameof(EnemyBombThrower.Update))] // Cleanup Crew
        [HarmonyPrefix]
        private static void Update_Prefix(Enemy ___enemy, ref PlayerAvatar ___playerTarget) {
            EnemyNoTargetHelpers.ClearTarget(ref ___playerTarget, ___enemy);
        }

        [HarmonyPatch(typeof(EnemyBombThrower), nameof(EnemyBombThrower.UpdatePlayerTarget))] // Cleanup Crew
        [HarmonyPrefix]
        private static bool UpdatePlayerTarget_Prefix(PlayerAvatar _player) {
            return EnemyNoTargetHelpers.Allows(_player);
        }

        [HarmonyPatch(typeof(EnemyBombThrowerHead), nameof(EnemyBombThrowerHead.Update))] // Cleanup Crew detached head
        [HarmonyPrefix]
        private static void HeadUpdate_Prefix(ref PlayerAvatar ___playerTarget) {
            ClearHeadTarget(ref ___playerTarget);
        }

        [HarmonyPatch(typeof(EnemyBombThrowerHead), nameof(EnemyBombThrowerHead.FixedUpdate))] // Cleanup Crew detached head
        [HarmonyPrefix]
        private static void HeadFixedUpdate_Prefix(ref PlayerAvatar ___playerTarget) {
            ClearHeadTarget(ref ___playerTarget);
        }

        [HarmonyPatch(typeof(EnemyBombThrowerHead), nameof(EnemyBombThrowerHead.UpdatePlayerTarget))] // Cleanup Crew detached head
        [HarmonyPrefix]
        private static void HeadUpdatePlayerTarget_Prefix(EnemyBombThrowerHead __instance, ref PlayerAvatar _player) {
            if (EnemyNoTargetHelpers.IsNoTarget(_player))
                _player = FindClosestAllowedPlayer(__instance);
        }

        private static void ClearHeadTarget(ref PlayerAvatar playerTarget) {
            if (EnemyNoTargetHelpers.IsNoTarget(playerTarget))
                playerTarget = null;
        }

        private static PlayerAvatar FindClosestAllowedPlayer(EnemyBombThrowerHead head) {
            PlayerAvatar closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (PlayerAvatar player in SemiFunc.PlayerGetList()) {
                if (player == null || player.isDisabled || !EnemyNoTargetHelpers.Allows(player))
                    continue;

                float distance = Vector3.Distance(head.transform.position, player.transform.position);
                if (distance >= closestDistance)
                    continue;

                closestPlayer = player;
                closestDistance = distance;
            }

            return closestPlayer;
        }
    }
}
