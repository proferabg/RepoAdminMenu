using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RepoAdminMenu.Utils {
    internal static class ReflectionUtil {
        private static readonly Dictionary<string, FieldInfo> _fieldCache = new Dictionary<string, FieldInfo>();

        private static FieldInfo GetField(Type type, string name) {
            string key = type.FullName + "." + name;
            if (!_fieldCache.TryGetValue(key, out var fi)) {
                fi = AccessTools.Field(type, name);
                _fieldCache[key] = fi;
            }
            return fi;
        }

        public static T GetFieldValue<T>(object obj, string fieldName) {
            var fi = GetField(obj.GetType(), fieldName);
            return (T)fi.GetValue(obj);
        }

        public static void SetFieldValue(object obj, string fieldName, object value) {
            var fi = GetField(obj.GetType(), fieldName);
            fi.SetValue(obj, value);
        }

        // Specific helpers for commonly accessed fields
        public static bool PlayerAvatar_GetDeadSet(PlayerAvatar avatar) => GetFieldValue<bool>(avatar, "deadSet");
        public static void PlayerAvatar_SetDeadSet(PlayerAvatar avatar, bool value) => SetFieldValue(avatar, "deadSet", value);

        public static PlayerDeathHead PlayerAvatar_GetDeathHead(PlayerAvatar avatar) => GetFieldValue<PlayerDeathHead>(avatar, "playerDeathHead");

        public static PlayerTumble PlayerAvatar_GetTumble(PlayerAvatar avatar) => GetFieldValue<PlayerTumble>(avatar, "tumble");

        public static int PlayerHealth_GetMaxHealth(PlayerHealth ph) => GetFieldValue<int>(ph, "maxHealth");
        public static void PlayerHealth_SetMaxHealth(PlayerHealth ph, int value) => SetFieldValue(ph, "maxHealth", value);
        public static int PlayerHealth_GetHealth(PlayerHealth ph) => GetFieldValue<int>(ph, "health");
        public static void PlayerHealth_SetHealth(PlayerHealth ph, int value) => SetFieldValue(ph, "health", value);

        public static int PlayerController_GetJumpExtra(PlayerController pc) => GetFieldValue<int>(pc, "JumpExtra");
        public static void PlayerController_SetJumpExtra(PlayerController pc, int value) => SetFieldValue(pc, "JumpExtra", value);

        public static bool PlayerDeathHead_GetInExtractionPoint(PlayerDeathHead pdh) => GetFieldValue<bool>(pdh, "inExtractionPoint");
        public static void PlayerDeathHead_SetInExtractionPoint(PlayerDeathHead pdh, bool value) => SetFieldValue(pdh, "inExtractionPoint", value);

        public static int PlayerAvatar_GetUpgradeMapPlayerCount(PlayerAvatar avatar) => GetFieldValue<int>(avatar, "upgradeMapPlayerCount");
        public static void PlayerAvatar_SetUpgradeMapPlayerCount(PlayerAvatar avatar, int value) => SetFieldValue(avatar, "upgradeMapPlayerCount", value);

        public static float PlayerAvatar_GetUpgradeCrouchRest(PlayerAvatar avatar) => GetFieldValue<float>(avatar, "upgradeCrouchRest");
        public static void PlayerAvatar_SetUpgradeCrouchRest(PlayerAvatar avatar, float value) => SetFieldValue(avatar, "upgradeCrouchRest", value);

        public static float PlayerAvatar_GetUpgradeTumbleWings(PlayerAvatar avatar) => GetFieldValue<float>(avatar, "upgradeTumbleWings");
        public static void PlayerAvatar_SetUpgradeTumbleWings(PlayerAvatar avatar, float value) => SetFieldValue(avatar, "upgradeTumbleWings", value);

        public static float PlayerAvatar_GetUpgradeTumbleClimb(PlayerAvatar avatar) => GetFieldValue<float>(avatar, "upgradeTumbleClimb");
        public static void PlayerAvatar_SetUpgradeTumbleClimb(PlayerAvatar avatar, float value) => SetFieldValue(avatar, "upgradeTumbleClimb", value);

        public static float PlayerAvatar_GetUpgradeDeathHeadBattery(PlayerAvatar avatar) => GetFieldValue<float>(avatar, "upgradeDeathHeadBattery");
        public static void PlayerAvatar_SetUpgradeDeathHeadBattery(PlayerAvatar avatar, float value) => SetFieldValue(avatar, "upgradeDeathHeadBattery", value);
    }
}
