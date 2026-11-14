namespace RepoAdminMenu.Patches.Enemies {

    internal static class EnemyNoTargetHelpers {

        internal static bool IsNoTarget(PlayerAvatar player) {
            return player != null && Settings.isNoTarget(player);
        }

        internal static bool Allows(PlayerAvatar player) {
            return !IsNoTarget(player);
        }

        internal static bool HasAllowedTarget(PlayerAvatar player) {
            return player != null && !Settings.isNoTarget(player);
        }

        internal static bool ClearTarget(ref PlayerAvatar player, Enemy enemy) {
            if (!IsNoTarget(player))
                return false;

            player = null;
            if (enemy != null) {
                enemy.CurrentState = EnemyState.Roaming;
                enemy.DisableChase(1f);
            }
            return true;
        }
    }
}
