using Newtonsoft.Json;
using Photon.Realtime;
using RepoAdminMenu.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RepoAdminMenu {
    internal class Settings {

        internal static Settings instance;

        public Settings() {
            instance = this;
        }

        public static void Init() {
            if (instance == null) {
                new Settings();
            }
        }

        public bool infiniteMoney = false,
            noBreak = false,
            noBatteryDrain = false,
            noTraps = false,
            weakEnemies = false,
            deafEnemies = false,
            blindEnemies = false,
            boomhammer = false,
            friendlyDuck = false,
            useShopUpgrades = false;


        public List<string> godModePlayers = new List<string>(),
            noDeathPlayers = new List<string>(),
            infiniteStaminaPlayers = new List<string>(),
            noTargetPlayers = new List<string>(),
            noTumblePlayers = new List<string>();

        public Dictionary<string, long> forcedTumble = new Dictionary<string, long>();

        public static void toggle(List<string> list, string steamId, bool value) {
            if (value)
                list.Remove(steamId);
            else
                list.Add(steamId);
            UpdateClients();
        }

        public static void toggleDictLong(Dictionary<string, long> list, string steamId, bool value) {
            if (value)
                list.Remove(steamId);
            else
                list.Add(steamId, 0);
            UpdateClients();
        }

        public static bool isGod(PlayerAvatar avatar) {
            return instance.godModePlayers.Contains(SemiFunc.PlayerGetSteamID(avatar));
        }

        public static bool isNoDeath(PlayerAvatar avatar) {
            return instance.noDeathPlayers.Contains(SemiFunc.PlayerGetSteamID(avatar));
        }

        public static bool isInfiniteStamina(PlayerAvatar avatar) {
            return instance.infiniteStaminaPlayers.Contains(SemiFunc.PlayerGetSteamID(avatar));
        }

        public static bool isNoTarget(PlayerAvatar avatar) {
            return instance.noTargetPlayers.Contains(SemiFunc.PlayerGetSteamID(avatar));
        }

        public static bool isNoTumble(PlayerAvatar avatar) {
            return instance.noTumblePlayers.Contains(SemiFunc.PlayerGetSteamID(avatar));
        }

        public static bool isForceTumble(PlayerAvatar avatar) {
            return instance.forcedTumble.ContainsKey(SemiFunc.PlayerGetSteamID(avatar));
        }
        internal static long getLastForceTumble(PlayerAvatar avatar) {
            return instance.forcedTumble.GetValueOrDefault(SemiFunc.PlayerGetSteamID(avatar), 0);
        }

        internal static long setLastForceTumble(PlayerAvatar avatar, long lastTumble) {
            return instance.forcedTumble[SemiFunc.PlayerGetSteamID(avatar)] = lastTumble;
        }

        public static void UpdateClients() {
            if (SemiFunc.IsMasterClient()) {
                RepoAdminMenu.mls.LogInfo("Sending Settings Sync");
                NetworkUtil.SendCommandString("SettingsSync", instance.toJson(), ReceiverGroup.Others);
                //instance.log();
            }
        }


        public string toJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static void loadFromJson(string json) {
            instance = JsonConvert.DeserializeObject<Settings>(json);
            //instance.log();
        }

        internal void log() {
            RepoAdminMenu.mls.LogInfo("\n\n\n" + instance.toJson() + "\n\n\n");
            RepoAdminMenu.mls.LogInfo("Current Settings:\n" +
                "  infiniteMoney - " + instance.infiniteMoney + "\n" +
                "  noBreak - " + instance.noBreak + "\n" +
                "  noBatteryDrain - " + instance.noBatteryDrain + "\n" +
                "  noTraps - " + instance.noTraps + "\n" +
                "  weakEnemies - " + instance.weakEnemies + "\n" +
                "  deafEnemies - " + instance.deafEnemies + "\n" +
                "  blindEnemies - " + instance.blindEnemies + "\n" +
                "  boomhammer - " + instance.boomhammer + "\n" +
                "  friendlyDuck - " + instance.friendlyDuck + "\n" +
                "  useShopUpgrades - " + instance.useShopUpgrades + "\n" +
                "  godMode: - " + string.Join(",", instance.godModePlayers.Select(player => SemiFunc.PlayerGetName(SemiFunc.PlayerGetFromSteamID(player)))) + "\n" +
                "  noDeathPlayers: - " + string.Join(",", instance.noDeathPlayers.Select(player => SemiFunc.PlayerGetName(SemiFunc.PlayerGetFromSteamID(player)))) + "\n" +
                "  infiniteStaminaPlayers: - " + string.Join(",", instance.infiniteStaminaPlayers.Select(player => SemiFunc.PlayerGetName(SemiFunc.PlayerGetFromSteamID(player)))) + "\n" +
                "  noTargetPlayers: - " + string.Join(",", instance.noTargetPlayers.Select(player => SemiFunc.PlayerGetName(SemiFunc.PlayerGetFromSteamID(player)))) + "\n" +
                "  noTumblePlayers: - " + string.Join(",", instance.noTumblePlayers.Select(player => SemiFunc.PlayerGetName(SemiFunc.PlayerGetFromSteamID(player)))) + "\n"
            );
        }

    }
}
