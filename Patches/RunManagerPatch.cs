using HarmonyLib;
using RepoAdminMenu.Utils;
using System.Linq;
using UnityEngine;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(RunManager))]
    internal class RunManagerPatch {

        private static bool lastKeyState = false;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void Update_Postfix(RunManager __instance) {
            if (Configuration.EnableHotkey.Value) {
                bool currentlyPressed = Input.GetKey(Configuration.MenuHotkey.Value);
                if (!lastKeyState && currentlyPressed) {
                    Menu.toggleMenu();
                    lastKeyState = currentlyPressed;
                } else if (lastKeyState && !currentlyPressed) {
                    lastKeyState = currentlyPressed;
                }
            }
        }

        [HarmonyPatch("ChangeLevel")]
        [HarmonyPostfix]
        private static void ChangeLevel_Postfix() {
            ItemUtil.Init();
            ValuableUtil.Init();
            EnemyUtil.Init();
            MapUtil.Init();
        }

        [HarmonyPatch("SetRunLevel")]
        [HarmonyPrefix]
        private static bool SetRunLevel_Prefix(RunManager __instance, ref Level ___previousRunLevel, ref Level ___levelCurrent) {
            MapUtil.resetLevels();

            // if no levels, set first level as enabled again
            if (__instance.levels.Count == 0) {
                MapUtil.setMapEnabled(MapUtil.getMaps().First().Value, true);
                MapUtil.resetLevels();
            }

            if(MapUtil.getEnabledCount() == 1 || MapUtil.getNextLevel() != null) {
                ___previousRunLevel = null!;
            }

            if (MapUtil.getNextLevel() != null) {
                ___levelCurrent = MapUtil.getNextLevel();
                MapUtil.clearNextLevel();
                return false;
            }

            return true;
        }
    }
}
