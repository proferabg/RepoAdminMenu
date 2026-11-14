using HarmonyLib;
using RepoAdminMenu.Utils;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal static class LocalizationManagerPatch {

        [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.Start))]
        [HarmonyPostfix]
        private static void Start_Postfix() {
            LanguageUtil.ReloadFromGameLocale();
        }

        [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.NotifyLocalizationChanged))]
        [HarmonyPostfix]
        private static void NotifyLocalizationChanged_Postfix() {
            LanguageUtil.ReloadFromGameLocale();
        }
    }
}
