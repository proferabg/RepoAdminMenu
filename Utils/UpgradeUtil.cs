using Photon.Realtime;
using REPOLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RepoAdminMenu.Utils {
    internal class UpgradeUtil {

        private static SortedDictionary<string, PlayerUpgrade> modUpgrades = new SortedDictionary<string, PlayerUpgrade>();
        private static SortedDictionary<string, GameUpgrade> gameUpgrades = new SortedDictionary<string, GameUpgrade>() {
            { "playerUpgradeCrouchRest", new GameUpgrade("playerUpgradeCrouchRest", "Crouch Rest", PunManager.instance.UpdateCrouchRestRightAway) },
            { "playerUpgradeHealth", new GameUpgrade("playerUpgradeHealth", "Health", PunManager.instance.UpdateCrouchRestRightAway) },
            { "playerUpgradeExtraJump", new GameUpgrade("playerUpgradeExtraJump", "Jump", PunManager.instance.UpdateExtraJumpRightAway) },
            { "playerUpgradeLaunch", new GameUpgrade("playerUpgradeLaunch", "Launch", PunManager.instance.UpdateTumbleLaunchRightAway) },
            { "playerUpgradeMapPlayerCount", new GameUpgrade("playerUpgradeMapPlayerCount", "Map Player Count", PunManager.instance.UpdateMapPlayerCountRightAway) },
            { "playerUpgradeRange", new GameUpgrade("playerUpgradeRange", "Range", PunManager.instance.UpdateGrabRangeRightAway) },
            { "playerUpgradeSpeed", new GameUpgrade("playerUpgradeSpeed", "Speed", PunManager.instance.UpdateSprintSpeedRightAway) },
            { "playerUpgradeStamina", new GameUpgrade("playerUpgradeStamina", "Stamina", PunManager.instance.UpdateEnergyRightAway) },
            { "playerUpgradeStrength", new GameUpgrade("playerUpgradeStrength", "Strength", PunManager.instance.UpdateGrabStrengthRightAway) },
            { "playerUpgradeThrow", new GameUpgrade("playerUpgradeThrow", "Throw", PunManager.instance.UpdateThrowStrengthRightAway) },
            { "playerUpgradeTumbleWings", new GameUpgrade("playerUpgradeTumbleWings", "Tumble Wings", PunManager.instance.UpdateTumbleWingsRightAway) },
            { "playerUpgradeTumbleClimb", new GameUpgrade("playerUpgradeTumbleClimb", "Tumble Climb", PunManager.instance.UpdateTumbleClimbRightAway) },
            { "playerUpgradeDeathHeadBattery", new GameUpgrade("playerUpgradeDeathHeadBattery", "Death Head Battery", PunManager.instance.UpdateDeathHeadBatteryRightAway) },
        };

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
                if (statsManager.dictionaryOfDictionaries.ContainsKey(type)) {
                    Dictionary<string, int> upgradeType = statsManager.dictionaryOfDictionaries[type];
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
            Dictionary<string, Dictionary<string, int>> dicts = StatsManager.instance.dictionaryOfDictionaries;

            if (!dicts.ContainsKey(key)) {
                dicts.Add(key, new Dictionary<string, int>());
            }

            Dictionary<string, int> dict = new Dictionary<string, int>();

            if (dicts.TryGetValue(key, out dict)) {
                dict[steamId] = level;
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

            public GameUpgrade(string id, string name, Action<string, int> punManagerFunction) {
                this.id = id;
                this.name = name;
                this.punManagerFunction = punManagerFunction;
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
                NetworkUtil.SendCommandSteamIDStringInt("UpgradeSync", avatar.steamID, id, level, ReceiverGroup.All);
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
