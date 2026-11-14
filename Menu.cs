using MenuLib;
using MenuLib.MonoBehaviors;
using RepoAdminMenu.Utils;
using REPOLib.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu {
    internal class Menu {

        private static string selectedPlayerId;

        private static REPOPopupPage currentMenu;

        private static string currentMenuStr = "";

        private static Dictionary<string, Action> menus = new Dictionary<string, Action>();

        //private static MethodInfo removeAllPagesMethod = AccessTools.Method(typeof(MenuManager), "PageCloseAll");

        private static Dictionary<string, System.Action<string, REPOPopupPage>> menuPreCallbacks = new Dictionary<string, System.Action<string, REPOPopupPage>>();
        private static Dictionary<string, System.Action<string, REPOPopupPage>> menuPostCallbacks = new Dictionary<string, System.Action<string, REPOPopupPage>>();

        public static void Init() {
            registerMenu("playerList", openPlayerListMenu);
            registerMenu("player", openPlayerMenu);
            registerMenu("playerUpgrade", openPlayerUpgrades);
            registerMenu("playerTeleport", openPlayerTeleport);
            registerMenu("spawn", openSpawnMenu);
            registerMenu("spawnItem", openSpawnItemsMenu);
            registerMenu("spawnEnemy", openSpawnEnemyMenu);
            registerMenu("spawnValuable", openSpawnValuablesMenu);
            registerMenu("spawnCosmetic", openSpawnCosmeticsMenu);
            registerMenu("spawnValuableTiny", openSpawnValuablesTinyMenu);
            registerMenu("spawnValuableSmall", openSpawnValuablesSmallMenu);
            registerMenu("spawnValuableMedium", openSpawnValuablesMediumMenu);
            registerMenu("spawnValuableBig", openSpawnValuablesBigMenu);
            registerMenu("spawnValuableWide", openSpawnValuablesWideMenu);
            registerMenu("spawnValuableTall", openSpawnValuablesTallMenu);
            registerMenu("spawnValuableVeryTall", openSpawnValuablesVeryTallMenu);
            registerMenu("map", openMapMenu);
            registerMenu("levelSelector", openLevelSelectorMenu);
            registerMenu("settings", openSettingsMenu);
            registerMenu("credits", openCreditsMenu);
            registerMenu("mainmenu", openMainMenu);
        }

        public static void registerMenu(string name, Action openAction) {
            menus.Add(name, openAction);
        }

        public static void addMenuPreCallback(string mod_name, System.Action<string, REPOPopupPage> action) {
            menuPreCallbacks.Add(mod_name, action);
        }

        public static void removeMenuPreCallback(string mod_name) {
            menuPreCallbacks.Remove(mod_name);
        }

        public static void addMenuPostCallback(string mod_name, System.Action<string, REPOPopupPage> action) {
            menuPostCallbacks.Add(mod_name, action);
        }

        public static void removeMenuPostCallback(string mod_name) {
            menuPostCallbacks.Remove(mod_name);
        }

        public static void toggleMenu() {
            if (menus.Count < 1)
                Init();


            if (SemiFunc.MenuLevel()) {
                RepoAdminMenu.mls.LogInfo("Repo Admin Menu can only be opened while in-game!");
                return;
            }

            if (!SemiFunc.IsMasterClientOrSingleplayer()) {
                RepoAdminMenu.mls.LogInfo("Repo Admin Menu can only be opened in single player or as host in multiplayer!");
                return;
            }

            if (currentMenu != null && currentMenu.isActiveAndEnabled) {
                closePage(currentMenu);
            } else {
                RepoAdminMenu.mls.LogInfo("Opening menu");
                menus.GetValueOrDefault(currentMenuStr, openMainMenu).Invoke();
            }
        }

        public static void closePage(REPOPopupPage page) {
            RepoAdminMenu.mls.LogInfo("Closing: " + page.menuPage.name);
            MenuManager.instance.PageCloseAll();
            page.ClosePage(true);
            MenuManager.instance.PageRemove(page.menuPage);
            if (currentMenu == page)
                currentMenu = null;
        }

        public static void navigate(REPOPopupPage page, string menu) {
            closePage(page);
            menus.GetValueOrDefault(menu, () => { RepoAdminMenu.mls.LogError("Menu not found: " + menu);  }).Invoke();
        }

        public static void openPage(REPOPopupPage page, string name) {
            MenuManager.instance.StartCoroutine(openPageInternal(page, name));
        }


        private static System.Collections.IEnumerator openPageInternal(REPOPopupPage page, string name) {
            yield return new WaitForSeconds(0.050f);
            foreach (KeyValuePair<string, System.Action<string, REPOPopupPage>> entry in menuPostCallbacks) {
                RepoAdminMenu.mls.LogInfo("Running post-callback for '" + entry.Key + "' on menu '" + name + "'");
                entry.Value.Invoke(name, page);
            }
            RepoAdminMenu.mls.LogInfo("Opening: " + page.menuPage.name);
            MenuManager.instance.PageCloseAll();
            //removeAllPagesMethod.Invoke(MenuManager.instance, new object[] { });
            page.OpenPage(false);
            currentMenu = page;
            currentMenuStr = name;
        }

        public static REPOPopupPage createMenu(string title, string currentMenu, string parentMenu) {
            var elem = MenuAPI.CreateREPOPopupPage("R.A.M. - " + title, REPOPopupPage.PresetSide.Left, false, true);
            foreach (KeyValuePair<string, System.Action<string, REPOPopupPage>> entry in menuPreCallbacks) {
                RepoAdminMenu.mls.LogInfo("Running pre-callback for '" + entry.Key + "' on menu '" + currentMenu + "'");
                entry.Value.Invoke(currentMenu, elem);
            }
            addBackButton(elem, parentMenu);
            return elem;
        }

        public static REPOPopupPage createMainMenu(string title) {
            var elem = MenuAPI.CreateREPOPopupPage(title, REPOPopupPage.PresetSide.Left, false, true);
            foreach (KeyValuePair<string, System.Action<string, REPOPopupPage>> entry in menuPreCallbacks) {
                RepoAdminMenu.mls.LogInfo("Running pre-callback for '" + entry.Key + "' on menu 'mainmenu'");
                entry.Value.Invoke("mainmenu", elem);
            }
            addCloseButton(elem);
            return elem;
        }

        public static void addButton(REPOPopupPage parent, string text, Action action) {
            parent.AddElementToScrollView(scrollView => {
                var elem = MenuAPI.CreateREPOButton(text, action, scrollView);
                return elem.rectTransform;
            });
        }

        public static void addBackButton(REPOPopupPage parent, string parentMenu) {
            parent.AddElement(transform => {
                MenuAPI.CreateREPOButton(LanguageUtil.Get("common.back", "← Back"), () => { closePage(parent); menus.GetValueOrDefault(parentMenu, () => { RepoAdminMenu.mls.LogError("Menu not found: " + parentMenu); }).Invoke(); }, transform, new Vector2(250, 20));
            });
        }

        public static void addCloseButton(REPOPopupPage parent) {
            parent.AddElement(transform => {
                MenuAPI.CreateREPOButton(LanguageUtil.Get("common.close", "Close"), () => closePage(parent), transform, new Vector2(270, 20));
            });
        }

        public static void addToggle(REPOPopupPage parent, string text, System.Action<bool> action, bool defaultValue, string[] buttonText = null) {
            parent.AddElementToScrollView(scrollView => {
                string offText = buttonText == null || buttonText.Length < 2 ? LanguageUtil.Get("common.off", "Off") : buttonText[0];
                string onText = buttonText == null || buttonText.Length < 2 ? LanguageUtil.Get("common.on", "On") : buttonText[1];
                var elem = MenuAPI.CreateREPOToggle(text, action, scrollView, Vector2.zero, offText, onText, !defaultValue);
                return elem.rectTransform;
            });
        }
        public static void addLabel(REPOPopupPage parent, string text) {
            parent.AddElementToScrollView(scrollView => {
                var elem = MenuAPI.CreateREPOLabel(text, scrollView);
                return elem.rectTransform;
            });
        }

        public static void addIntSlider(REPOPopupPage parent, string text, string description, System.Action<int> action, int min, int max, int defaultValue) {
            parent.AddElementToScrollView(scrollView => {
                var elem = MenuAPI.CreateREPOSlider(text, description, action, scrollView, Vector2.zero, min, max, defaultValue, "", "", REPOSlider.BarBehavior.UpdateWithValue);
                return elem.rectTransform;
            });
        }

        public static void addFloatSlider(REPOPopupPage parent, string text, string description, System.Action<float> action, float min, float max, int precision, float defaultValue) {
            parent.AddElementToScrollView(scrollView => {
                var elem = MenuAPI.CreateREPOSlider(text, description, action, scrollView, Vector2.zero, min, max, precision, defaultValue, "", "", REPOSlider.BarBehavior.UpdateWithValue);
                return elem.rectTransform;
            });
        }

        public static void addStringSlider(REPOPopupPage parent, string text, string description, System.Action<string> action, string[] options, string defaultValue) {
            parent.AddElementToScrollView(scrollView => {
                var elem = MenuAPI.CreateREPOSlider(text, description, action, scrollView, options, defaultValue, Vector2.zero, "", "", REPOSlider.BarBehavior.UpdateWithValue);
                return elem.rectTransform;
            });
        }

        internal static void RefreshLocalization() {
            if (currentMenu == null || !currentMenu.isActiveAndEnabled)
                return;

            string menuName = currentMenuStr;
            closePage(currentMenu);
            menus.GetValueOrDefault(menuName, openMainMenu).Invoke();
        }

        public static void openMainMenu() {
            var mainMenu = createMainMenu("R.E.P.O. Admin Menu");

            addButton(mainMenu, LanguageUtil.Get("menu.main.players", "Players"), () => { navigate(mainMenu, "playerList"); });
            addButton(mainMenu, LanguageUtil.Get("menu.main.spawn", "Spawn"), () => { navigate(mainMenu, "spawn"); });
            addButton(mainMenu, LanguageUtil.Get("menu.main.map", "Map"), () => { navigate(mainMenu, "map"); });
            addButton(mainMenu, LanguageUtil.Get("menu.main.settings", "Settings"), () => { navigate(mainMenu, "settings"); });
            addButton(mainMenu, LanguageUtil.Get("menu.main.credits", "Credits"), () => { navigate(mainMenu, "credits"); });
            addButton(mainMenu, LanguageUtil.Get("menu.main.report_bug", "Report a Bug"), () => { Application.OpenURL("https://github.com/proferabg/RepoAdminMenu/issues/new?template=bug_report.yml"); Application.OpenURL("file://" + Application.persistentDataPath + "/Player.log"); });

            openPage(mainMenu, "mainmenu");
        }

        private static void openPlayerListMenu() {
            var playersMenu = createMenu(LanguageUtil.Get("menu.title.players", "Players"), "playerList", "mainmenu");

            foreach (PlayerAvatar player in SemiFunc.PlayerGetAll()) {
                addButton(playersMenu, SemiFunc.PlayerGetName(player), () => { selectedPlayerId = SemiFunc.PlayerGetSteamID(player); navigate(playersMenu, "player"); });
            }

            openPage(playersMenu, "playerList");
        }

        private static void openPlayerMenu() {
            if (selectedPlayerId == null || SemiFunc.PlayerGetFromSteamID(selectedPlayerId) == null) {
                RepoAdminMenu.mls.LogInfo("No player selected");
                openPlayerListMenu();
                return;
            }

            PlayerAvatar avatar = SemiFunc.PlayerGetFromSteamID(selectedPlayerId);

            var playerMenu = createMenu(SemiFunc.PlayerGetName(avatar), "player", "playerList");

            addToggle(playerMenu, LanguageUtil.Get("menu.player.god_mode", "God Mode"), (b) => { Settings.toggle(Settings.instance.godModePlayers, avatar.steamID, b); }, Settings.isGod(avatar));
            addToggle(playerMenu, LanguageUtil.Get("menu.player.no_death", "No Death"), (b) => { Settings.toggle(Settings.instance.noDeathPlayers, avatar.steamID, b); }, Settings.isNoDeath(avatar));
            addToggle(playerMenu, LanguageUtil.Get("menu.player.no_target", "No Target"), (b) => { Settings.toggle(Settings.instance.noTargetPlayers, avatar.steamID, b); }, Settings.isNoTarget(avatar));
            addToggle(playerMenu, LanguageUtil.Get("menu.player.no_tumble", "No Tumble"), (b) => { Settings.toggle(Settings.instance.noTumblePlayers, avatar.steamID, b); }, Settings.isNoTumble(avatar));
            addToggle(playerMenu, LanguageUtil.Get("menu.player.infinite_stamina", "Infinite Stamina"), (b) => { Settings.toggle(Settings.instance.infiniteStaminaPlayers, avatar.steamID, b); }, Settings.isInfiniteStamina(avatar));
            addToggle(playerMenu, LanguageUtil.Get("menu.player.force_tumble", "Force Tumble"), (b) => { Settings.toggleDictLong(Settings.instance.forcedTumble, avatar.steamID, b); }, Settings.isForceTumble(avatar));
            addButton(playerMenu, LanguageUtil.Get("menu.player.upgrades", "Upgrades"), () => { navigate(playerMenu, "playerUpgrade"); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.heal", "Heal"), () => { PlayerUtil.healPlayer(avatar); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.kill", "Kill"), () => { PlayerUtil.killPlayer(avatar); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.revive", "Revive"), () => { PlayerUtil.revivePlayer(avatar); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.teleport", "Teleport"), () => { navigate(playerMenu, "playerTeleport"); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.give_crown", "Give Crown"), () => { PlayerUtil.giveCrown(avatar); });
            addButton(playerMenu, LanguageUtil.Get("menu.player.kick", "Kick Player"), () => {
                closePage(playerMenu);
                openConfirmMenu("player", LanguageUtil.Format("confirm.kick", "Kick: {0}", SemiFunc.PlayerGetName(avatar)), new Dictionary<string, System.Action> {
                    {LanguageUtil.Get("common.yes", "Yes"), () => { PlayerUtil.KickPlayer(avatar); }},
                    {LanguageUtil.Get("common.no", "No"), null}
                });
            });

            addButton(playerMenu, LanguageUtil.Get("menu.player.ban", "Ban Player"), () => {
                closePage(playerMenu);
                openConfirmMenu("player", LanguageUtil.Format("confirm.ban", "Ban: {0}", SemiFunc.PlayerGetName(avatar)), new Dictionary<string, System.Action> {
                    {LanguageUtil.Get("common.yes", "Yes"), () => {PlayerUtil.BanPlayer(avatar); }},
                    {LanguageUtil.Get("common.no", "No"), null}
                });
            });

            openPage(playerMenu, "player");
        }

        private static void openPlayerUpgrades() {
            if (selectedPlayerId == null || SemiFunc.PlayerGetFromSteamID(selectedPlayerId) == null) {
                RepoAdminMenu.mls.LogInfo("No player selected");
                openPlayerListMenu();
                return;
            }

            PlayerAvatar avatar = SemiFunc.PlayerGetFromSteamID(selectedPlayerId);

            var upgradesMenu = createMenu(LanguageUtil.Format("menu.title.player_upgrades", "{0} - Upgrades", SemiFunc.PlayerGetName(avatar)), "playerUpgrade", "player");

            foreach(KeyValuePair<string, UpgradeUtil.GameUpgrade> upgradePair in UpgradeUtil.GetGameUpgrades()) {
                addIntSlider(upgradesMenu, upgradePair.Value.GetName(), "", (v) => { upgradePair.Value.Upgrade(avatar, v); }, 0, Configuration.MaxUpgradeLevel.Value, upgradePair.Value.GetPlayerLevel(avatar));
            }

            foreach (KeyValuePair<string, PlayerUpgrade> upgradePair in UpgradeUtil.GetModUpgrades()) {
                addIntSlider(upgradesMenu, LanguageUtil.Format("common.mod_name", "(Mod) {0}", upgradePair.Key), "", (v) => { UpgradeUtil.UpgradeLevel(upgradePair.Value, avatar, v); }, 0, Configuration.MaxUpgradeLevel.Value, upgradePair.Value.GetLevel(avatar));
            }


            openPage(upgradesMenu, "playerUpgrade");
        }

        private static void openPlayerTeleport() {
            if (selectedPlayerId == null || SemiFunc.PlayerGetFromSteamID(selectedPlayerId) == null) {
                RepoAdminMenu.mls.LogInfo("No player selected");
                openPlayerListMenu();
                return;
            }

            PlayerAvatar avatar = SemiFunc.PlayerGetFromSteamID(selectedPlayerId);

            var teleportMenu = createMenu(LanguageUtil.Format("menu.title.player_teleport", "{0} - Teleport Menu", SemiFunc.PlayerGetName(avatar)), "playerTeleport", "player");

            if (avatar != PlayerAvatar.instance) {
                addButton(teleportMenu, LanguageUtil.Format("menu.teleport.go_to", "Go to {0}", SemiFunc.PlayerGetName(avatar)), () => { PlayerUtil.teleportTo(avatar); });
                addButton(teleportMenu, LanguageUtil.Get("menu.teleport.summon", "Summon"), () => { PlayerUtil.summon(avatar); });
            }
            addButton(teleportMenu, LanguageUtil.Get("menu.teleport.return_to_truck", "Return To Truck"), () => { PlayerUtil.returnToTruck(avatar); });

            foreach (PlayerAvatar player in SemiFunc.PlayerGetAll()) {
                if (player != avatar && player != PlayerAvatar.instance) {
                    addButton(teleportMenu, LanguageUtil.Format("menu.teleport.send_to", "Send to {0}", SemiFunc.PlayerGetName(player)), () => { PlayerUtil.teleport(avatar, player); });
                }
            }

            openPage(teleportMenu, "playerTeleport");
        }

        private static void openSpawnMenu() {
            var spawnerMenu = createMenu(LanguageUtil.Get("menu.title.spawn", "Spawn"), "spawn", "mainmenu");

            addButton(spawnerMenu, LanguageUtil.Get("menu.spawn.items", "Items"), () => { navigate(spawnerMenu, "spawnItem"); });
            addButton(spawnerMenu, LanguageUtil.Get("menu.spawn.valuables", "Valuables"), () => { navigate(spawnerMenu, "spawnValuable"); });
            addButton(spawnerMenu, LanguageUtil.Get("menu.spawn.enemies", "Enemies"), () => { navigate(spawnerMenu, "spawnEnemy"); });
            addButton(spawnerMenu, LanguageUtil.Get("menu.spawn.cosmetic_boxes", "Cosmetic Boxes"), () => { navigate(spawnerMenu, "spawnCosmetic"); });
            addIntSlider(spawnerMenu, LanguageUtil.Get("menu.spawn.count", "Spawner Count"), "", (v) => { Settings.instance.spawnerCount = v; }, 1, Configuration.MaxSpawnCount.Value, Settings.instance.spawnerCount);

            openPage(spawnerMenu, "spawn");
        }

        private static void openSpawnItemsMenu() {
            var itemsMenu = createMenu(LanguageUtil.Get("menu.title.spawn_items", "Spawn - Items"), "spawnItem", "spawn");

            foreach (KeyValuePair<string, Item> entry in ItemUtil.getItems()) {
                addButton(itemsMenu, entry.Key, () => ItemUtil.spawnItem(entry.Value));
            }

            openPage(itemsMenu, "spawnItem");
        }

        private static void openSpawnValuablesMenu() {
            var valuablesMenu = createMenu(LanguageUtil.Get("menu.title.spawn_valuables", "Spawn - Valuables"), "spawnValuable", "spawn");


            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.tiny", "Tiny"), () => { navigate(valuablesMenu, "spawnValuableTiny"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.small", "Small"), () => { navigate(valuablesMenu, "spawnValuableSmall"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.medium", "Medium"), () => { navigate(valuablesMenu, "spawnValuableMedium"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.big", "Big"), () => { navigate(valuablesMenu, "spawnValuableBig"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.wide", "Wide"), () => { navigate(valuablesMenu, "spawnValuableWide"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.tall", "Tall"), () => { navigate(valuablesMenu, "spawnValuableTall"); });
            addButton(valuablesMenu, LanguageUtil.Get("menu.valuable_size.very_tall", "Very Tall"), () => { navigate(valuablesMenu, "spawnValuableVeryTall"); });


            openPage(valuablesMenu, "spawnValuable");
        }

        private static void openSpawnValuablesTinyMenu() {
            var valuablesTinyMenu = createMenu(LanguageUtil.Get("menu.title.tiny_valuables", "Tiny Valuables"), "spawnValuableTiny", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getTinyValuables()) {
                addButton(valuablesTinyMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesTinyMenu, "spawnValuableTiny");
        }

        private static void openSpawnValuablesSmallMenu() {
            var valuablesSmallMenu = createMenu(LanguageUtil.Get("menu.title.small_valuables", "Small Valuables"), "spawnValuableSmall", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getSmallValuables()) {
                addButton(valuablesSmallMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesSmallMenu, "spawnValuableSmall");
        }

        private static void openSpawnValuablesMediumMenu() {
            var valuablesMediumMenu = createMenu(LanguageUtil.Get("menu.title.medium_valuables", "Medium Valuables"), "spawnValuableMedium", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getMediumValuables()) {
                addButton(valuablesMediumMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesMediumMenu, "spawnValuableMedium");
        }

        private static void openSpawnValuablesBigMenu() {
            var valuablesBigMenu = createMenu(LanguageUtil.Get("menu.title.big_valuables", "Big Valuables"), "spawnValuableBig", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getBigValuables()) {
                addButton(valuablesBigMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesBigMenu, "spawnValuableBig");
        }

        private static void openSpawnValuablesWideMenu() {
            var valuablesWideMenu = createMenu(LanguageUtil.Get("menu.title.wide_valuables", "Wide Valuables"), "spawnValuableWide", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getWideValuables()) {
                addButton(valuablesWideMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesWideMenu, "spawnValuableWide");
        }

        private static void openSpawnValuablesTallMenu() {
            var valuablesTallMenu = createMenu(LanguageUtil.Get("menu.title.tall_valuables", "Tall Valuables"), "spawnValuableTall", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getTallValuables()) {
                addButton(valuablesTallMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesTallMenu, "spawnValuableTall");
        }

        private static void openSpawnValuablesVeryTallMenu() {
            var valuablesVeryTallMenu = createMenu(LanguageUtil.Get("menu.title.very_tall_valuables", "Very Tall Valuables"), "spawnValuableVeryTall", "spawnValuable");

            foreach (KeyValuePair<string, PrefabRef> entry in ValuableUtil.getVeryTallValuables()) {
                addButton(valuablesVeryTallMenu, entry.Key, () => ValuableUtil.spawnValuable(entry.Value));
            }

            openPage(valuablesVeryTallMenu, "spawnValuableVeryTall");
        }

        private static void openSpawnCosmeticsMenu() {
            var cosmeticMenu = createMenu(LanguageUtil.Get("menu.title.spawn_cosmetics", "Spawn - Cosmetic Boxes"), "spawnCosmetic", "spawn");


            foreach (SemiFunc.Rarity rarity in System.Enum.GetValues(typeof(SemiFunc.Rarity))) {
                addButton(cosmeticMenu, LanguageUtil.Get("rarity." + rarity.ToString().ToLowerInvariant(), rarity.ToString()), () => CosmeticUtil.spawnCosmetic(rarity));
            }

            openPage(cosmeticMenu, "spawnCosmetic");
        }

        private static void openSpawnEnemyMenu() {
            var spawnerMenu = createMenu(LanguageUtil.Get("menu.title.spawn_enemies", "Spawn - Enemies"), "spawnEnemy", "spawn");

            foreach(string enemy in EnemyUtil.getEnemies().Keys) {
                addButton(spawnerMenu, enemy, () => EnemyUtil.spawnEnemy(enemy));
            }

            foreach (string enemy in EnemyUtil.getModEnemies().Keys) {
                addButton(spawnerMenu, LanguageUtil.Format("common.mod_name", "(Mod) {0}", enemy), () => EnemyUtil.spawnEnemy(enemy));
            }

            openPage(spawnerMenu, "spawnEnemy");
        }

        private static void openMapMenu() {
            var mapMenu = createMenu(LanguageUtil.Get("menu.title.map_settings", "Map Settings"), "map", "mainmenu");

            foreach(KeyValuePair<string, Level> entry in MapUtil.getMaps()) {
                addToggle(mapMenu, entry.Key, (b) => { MapUtil.setMapEnabled(entry.Value, b); }, MapUtil.isMapEnabled(entry.Value));
            }

            addIntSlider(mapMenu, LanguageUtil.Get("menu.map.current_level", "Current Level"), "", (v) => { RunManager.instance.levelsCompleted = v; }, 1, 500, RunManager.instance.levelsCompleted);
            addIntSlider(mapMenu, LanguageUtil.Get("menu.map.money", "Money"), "", (v) => { PunManager.instance.SetRunStatSet("currency", v); }, 0, 9999, StatsManager.instance.runStats["currency"]);
            addButton(mapMenu, LanguageUtil.Get("menu.map.selector", "Map Selector"), () => { navigate(mapMenu, "levelSelector"); });
            addButton(mapMenu, LanguageUtil.Get("menu.map.discover_extraction", "Discover Extraction Point"), ExtractionPointUtil.discoverNext);
            addButton(mapMenu, LanguageUtil.Get("menu.map.complete_extraction", "Complete Extraction Point"), ExtractionPointUtil.complete);

            openPage(mapMenu, "map");
        }

        private static void openLevelSelectorMenu() {
            var mapMenu = createMenu(LanguageUtil.Get("menu.title.map_selector", "Map Selector"), "levelSelector", "map");

            foreach (KeyValuePair<string, Level> entry in MapUtil.getMaps()) {
                addButton(mapMenu, entry.Key, () => { MapUtil.changeLevel(entry.Value); });
            }

            foreach (KeyValuePair<string, Level> entry in MapUtil.getArenas()) {
                addButton(mapMenu, entry.Key, () => { MapUtil.changeLevel(entry.Value); });
            }

            addButton(mapMenu, LanguageUtil.Get("menu.map.lobby", "Lobby"), () => { MapUtil.changeLevel(RunManager.instance.levelLobby); });

            foreach (KeyValuePair<string, Level> entry in MapUtil.getShops()) {
                addButton(mapMenu, entry.Key, () => { MapUtil.changeLevel(entry.Value); });
            }

            openPage(mapMenu, "levelSelector");
        }

        private static void openSettingsMenu() {
            var settingsMenu = createMenu(LanguageUtil.Get("menu.title.settings", "Settings"), "settings", "mainmenu");

            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.infinite_money", "Infinite Money"), (b) => { Settings.UpdateOption(ref Settings.instance.infiniteMoney, !b); }, Settings.instance.infiniteMoney);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.no_break", "No Break"), (b) => { Settings.UpdateOption(ref Settings.instance.noBreak, !b); }, Settings.instance.noBreak);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.no_battery_drain", "No Battery/Ammo Drain"), (b) => { Settings.UpdateOption(ref Settings.instance.noBatteryDrain, !b); }, Settings.instance.noBatteryDrain);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.no_traps", "No Traps"), (b) => { Settings.UpdateOption(ref Settings.instance.noTraps, !b); }, Settings.instance.noTraps);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.weak_enemies", "Weak Enemies"), (b) => { Settings.UpdateOption(ref Settings.instance.weakEnemies, !b); }, Settings.instance.weakEnemies);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.deaf_enemies", "Deaf Enemies"), (b) => { Settings.UpdateOption(ref Settings.instance.deafEnemies, !b); }, Settings.instance.deafEnemies);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.blind_enemies", "Blind Enemies"), (b) => { Settings.UpdateOption(ref Settings.instance.blindEnemies, !b); }, Settings.instance.blindEnemies);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.freeze_enemy_ai", "Freeze Enemy AI"), (b) => { Settings.UpdateOption(ref Settings.instance.freezeEnemyAI, !b); }, Settings.instance.freezeEnemyAI);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.boom_hammer", "Boom Hammer"), (b) => { Settings.UpdateOption(ref Settings.instance.boomhammer, !b); }, Settings.instance.boomhammer);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.friendly_duck", "Friendly Duck"), (b) => { Settings.UpdateOption(ref Settings.instance.friendlyDuck, !b); }, Settings.instance.friendlyDuck);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.disable_shopkeeper", "Disable Shopkeeper"), (b) => { Settings.UpdateOption(ref Settings.instance.disableShopKeeper, !b); }, Settings.instance.disableShopKeeper);
            addToggle(settingsMenu, LanguageUtil.Get("menu.settings.upgrade_in_shop", "Upgrade In Shop"), (b) => { Settings.UpdateOption(ref Settings.instance.useShopUpgrades, !b); }, Settings.instance.useShopUpgrades);

            openPage(settingsMenu, "settings");
        }

        private static void openCreditsMenu() {
            var creditsMenu = createMenu(LanguageUtil.Get("menu.title.credits", "Credits"), "credits", "mainmenu");

            addLabel(creditsMenu, LanguageUtil.Get("menu.credits.created_by", "Repo Admin Menu by"));
            addLabel(creditsMenu, "  proferabg  ");
            addLabel(creditsMenu, "");
            addLabel(creditsMenu, LanguageUtil.Get("menu.credits.special_thanks", "Special thanks to:"));
            addLabel(creditsMenu, " - REPOrium_Team");
            addLabel(creditsMenu, " - nickklmao");
            addLabel(creditsMenu, " - Godji");
            addLabel(creditsMenu, " - Zehs");
            addLabel(creditsMenu, "");
            addLabel(creditsMenu, LanguageUtil.Get("menu.credits.copyright", "Repo Admin Menu © 2026"));

            openPage(creditsMenu, "credits");
        }

        private static void openConfirmMenu(string currentMenu, string message, Dictionary<string, System.Action> options) {
            var confirmMenu = createMenu(LanguageUtil.Get("menu.title.confirm", "Confirm"), "confirm", "player");
            confirmMenu.onEscapePressed = () => false;

            addLabel(confirmMenu, message);
            addLabel(confirmMenu, "");
            foreach (KeyValuePair<string, System.Action> option in options) {
                addButton(confirmMenu, option.Key, () => { if(option.Value != null) option.Value.Invoke(); navigate(confirmMenu, currentMenu); });
            }

            openPage(confirmMenu, "confirm");
        }

        public static string getSelectedPlayer() {
            return selectedPlayerId;
        }
    }
}
