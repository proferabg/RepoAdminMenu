using REPOLib.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RepoAdminMenu.Utils {
    internal class UpgradeUtil {


        private static SortedDictionary<string, PlayerUpgrade> upgrades = new SortedDictionary<string, PlayerUpgrade>();

        public static void Init() {
            upgrades.Clear();
            foreach (PlayerUpgrade upgrade in Upgrades.PlayerUpgrades.ToList()) {
                string name = Regex.Replace(upgrade.UpgradeId, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                if (!upgrades.ContainsKey(name))
                    upgrades.Add(name, upgrade);
            }
        }

        public static SortedDictionary<string, PlayerUpgrade> getUpgrades() {
            return upgrades;
        }

        public static void UpgradeLevel(PlayerUpgrade upgrade, PlayerAvatar avatar, int level) {
            upgrade.SetLevel(avatar, level);
            RepoAdminMenu.mls.LogInfo("Upgrade: " + SemiFunc.PlayerGetName(avatar) + " - '" + upgrade.UpgradeId + "' -> " + level);
        }
    }
}
