using BepInEx.Configuration;
using UnityEngine;

namespace RepoAdminMenu {
    internal class Configuration {

        public static ConfigEntry<bool> EnableCommands { get; private set; }

        public static ConfigEntry<bool> EnableHotkey { get; private set; }

        public static ConfigEntry<KeyCode> MenuHotkey { get; private set; }

        public static ConfigEntry<int> MaxUpgradeLevel { get; private set; }

        public static void Init(ConfigFile config) {
            EnableCommands = config.Bind<bool>(
                "General",
                "EnableCommands",
                true,
                "Enables RepoAdminMenu commands"
            );

            EnableHotkey = config.Bind<bool>(
                "General",
                "EnableHotkey",
                true,
                "Enables RepoAdminMenu menu"
            );

            MenuHotkey = config.Bind(
                "General",
                "MenuHotkey",
                KeyCode.F8,
                "Key to open or close the menu"
            );

            MaxUpgradeLevel = config.Bind<int>(
                "General",
                "MaxUpgrade",
                25,
                "Changes the slider max value for player upgrades"
            );

        }
    }
}
