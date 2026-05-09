using HarmonyLib;
using Photon.Realtime;
using REPOLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RepoAdminMenu.Utils {
    internal class UpgradeUtil {

        private static SortedDictionary<string, PlayerUpgrade> modUpgrades = new SortedDictionary<string, PlayerUpgrade>();
        private static SortedDictionary<string, GameUpgrade> gameUpgrades = new SortedDictionary<string, GameUpgrade>() {
            { "playerUpgradeCrouchRest", new GameUpgrade("playerUpgradeCrouchRest", "Crouch Rest", "UpdateCrouchRestRightAway") },
            { "playerUpgradeHealth", new GameUpgrade("playerUpgradeHealth", "Health", "UpdateCrouchRestRightAway") },
            { "playerUpgradeExtraJump", new GameUpgrade("playerUpgradeExtraJump", "Jump", "UpdateExtraJumpRightAway") },
            { "playerUpgradeLaunch", new GameUpgrade("playerUpgradeLaunch", "Launch", "UpdateTumbleLaunchRightAway") },
            { "playerUpgradeMapPlayerCount", new GameUpgrade("playerUpgradeMapPlayerCount", "Map Player Count", "UpdateMapPlayerCountRightAway") },
            { "playerUpgradeRange", new GameUpgrade("playerUpgradeRange", "Range", "UpdateGrabRangeRightAway") },
            { "playerUpgradeSpeed", new GameUpgrade("playerUpgradeSpeed", "Speed", "UpdateSprintSpeedRightAway") },
            { "playerUpgradeStamina", new GameUpgrade("playerUpgradeStamina", "Stamina", "UpdateEnergyRightAway") },
            { "playerUpgradeStrength", new GameUpgrade("playerUpgradeStrength", "Strength", "UpdateGrabStrengthRightAway") },
            { "playerUpgradeThrow", new GameUpgrade("playerUpgradeThrow", "Throw", "UpdateThrowStrengthRightAway") },
            { "playerUpgradeTumbleWings", new GameUpgrade("playerUpgradeTumbleWings", "Tumble Wings", "UpdateTumbleWingsRightAway") },
            { "playerUpgradeTumbleClimb", new GameUpgrade("playerUpgradeTumbleClimb", "Tumble Climb", "UpdateTumbleClimbRightAway") },
            { "playerUpgradeDeathHeadBattery", new GameUpgrade("playerUpgradeDeathHeadBattery", "Death Head Battery", "UpdateDeathHeadBatteryRightAway") },
        };

        // Reflection helpers for accessing internal/private game fields
        private static readonly FieldInfo _dictionaryOfDictionariesField =
            AccessTools.Field(typeof(StatsManager), "dictionaryOfDictionaries");

        private static SortedDictionary<string, Dictionary<string, int>> GetDictOfDicts(StatsManager sm) {
            return (SortedDictionary<string, Dictionary<string, int>>)_dictionaryOfDictionariesField.GetValue(sm);
        }

        private static Action<string, int> CreatePunManagerAction(string methodName) {
            var method = AccessTools.Method(typeof(PunManager), methodName);
            return (steamId, level) => method.Invoke(PunManager.instance, new object[] { steamId, level });
        }

        public static void Init() {
            modUpgrades.Clear();
            foreach (PlayerUpgrade upgrade in Upgrades.PlayerUpgrades.ToList()) {
                string name = Regex.Replace(upgrade.UpgradeId, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                if (!modUpgrades.ContainsKey(name))
                    modUpgrades.Add(name, upgrade);
            }
        }

        public static SortedDictionary<string, GameUpgrade> GetGameUpgrades() {
            return gameUpgrades;
        }

        public static SortedDictionary<string, PlayerUpgrade> GetModUpgrades() {
            return modUpgrades;
        }

        public static void UpgradeLevel(PlayerUpgrade upgrade, PlayerAvatar avatar, int level) {
            upgrade.SetLevel(avatar, level);
            RepoAdminMenu.mls.LogInfo("Upgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + upgrade.UpgradeId + "' -> " + level);
        }
        public static int GetPlayerUpgradeLevel(string type, PlayerAvatar avatar) {
            StatsManager statsManager = StatsManager.instance;
            string playerSteamId = SemiFunc.PlayerGetSteamID(avatar);

            if (statsManager != null) {
                var dicts = GetDictOfDicts(statsManager);
                if (dicts != null && dicts.ContainsKey(type)) {
                    Dictionary<string, int> upgradeType = dicts[type];
                    if (upgradeType != null && upgradeType.ContainsKey(playerSteamId)) {
                        return upgradeType[playerSteamId];
                    }
                }
            }
            return 0;
        }

        public static void UpgradeSync(PlayerAvatar avatar, string key, int level) {
            RepoAdminMenu.mls.LogInfo("UpgradeSync: " + SemiFunc.PlayerGetName(avatar) + " - '" + key + "' -> " + level);
            string steamId = SemiFunc.PlayerGetSteamID(avatar);
            var dicts = GetDictOfDicts(StatsManager.instance);

            if (dicts != null) {
                if (!dicts.ContainsKey(key)) {
                    dicts.Add(key, new Dictionary<string, int>());
                }

                if (dicts.TryGetValue(key, out var dict)) {
                    dict[steamId] = level;
                }
            }

            // process upgrade locally
            if (avatar == PlayerAvatar.instance) {
                DoImmediateUpgrade(avatar, key, level);
            }
        }

        private static void DoImmediateUpgrade(PlayerAvatar avatar, string key, int level) {
            string steamId = SemiFunc.PlayerGetSteamID(avatar);
            RepoAdminMenu.mls.LogInfo("DoImmediateUpgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + key + "'");
            try {
                if (gameUpgrades.TryGetValue(key, out var value))
                    value.PunUpgrade(avatar, level);
            } catch { }
        }

        public class GameUpgrade {
            private string id;
            private string name;
            private Action<string, int> punManagerFunction;

            public GameUpgrade(string id, string name, string punManagerMethodName) {
                this.id = id;
                this.name = name;
                if (!string.IsNullOrEmpty(punManagerMethodName))
                    this.punManagerFunction = CreatePunManagerAction(punManagerMethodName);
            }

            public string GetID() {
                return id;
            }

            public string GetName() { 
                return name; 
            }

            public int GetPlayerLevel(PlayerAvatar avatar) {
                return GetPlayerUpgradeLevel(id, avatar);
            }

            public void Upgrade(PlayerAvatar avatar, int level) {
                RepoAdminMenu.mls.LogInfo("Upgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + id + "' -> " + level);
                if (SemiFunc.IsMultiplayer()) {
                    NetworkUtil.SendCommandSteamIDStringInt("UpgradeSync", SemiFunc.PlayerGetSteamID(avatar), id, level, ReceiverGroup.All);
                } else {
                    UpgradeSync(avatar, id, level);
                }
            }

            public void PunUpgrade(PlayerAvatar avatar, int level) {
                if (punManagerFunction != null) {
                    string steamId = SemiFunc.PlayerGetSteamID(avatar);
                    punManagerFunction.Invoke(steamId, level);
                 }

            }

        }

    }
}
