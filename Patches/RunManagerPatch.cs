using HarmonyLib;
using RepoAdminMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(RunManager))]
    internal class RunManagerPatch {

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix(RunManager __instance) {
            if(__instance != null) {
                __instance.gameObject.AddComponent<RepoAdminMenu>();
                RepoAdminMenu.mls.LogInfo("Registered RepoAdminMenu.Update() with R.E.P.O.");
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
