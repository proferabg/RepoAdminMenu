using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace RepoAdminMenu.Utils {

    internal static class LanguageUtil {

        private const string EnglishLanguageCode = "en";

        private static Dictionary<string, string> englishStrings = new Dictionary<string, string>(StringComparer.Ordinal);
        private static Dictionary<string, string> activeStrings = new Dictionary<string, string>(StringComparer.Ordinal);
        private static string requestedLanguageCode;
        private static bool initialized;

        internal static string ActiveLanguageCode { get; private set; } = EnglishLanguageCode;

        private static string LanguageDirectory {
            get {
                string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(assemblyDirectory ?? string.Empty, "Languages");
            }
        }

        internal static void Init() {
            ReloadFromGameLocale(false);
        }

        internal static void ReloadFromGameLocale(bool refreshMenu = true) {
            string gameLanguageCode = GetGameLanguageCode();
            if (initialized && string.Equals(gameLanguageCode, requestedLanguageCode, StringComparison.OrdinalIgnoreCase))
                return;

            englishStrings = LoadLanguageFile(EnglishLanguageCode, true);
            activeStrings = new Dictionary<string, string>(englishStrings, StringComparer.Ordinal);

            requestedLanguageCode = gameLanguageCode;
            ActiveLanguageCode = EnglishLanguageCode;
            string neutralLanguageCode = gameLanguageCode.Split('-')[0];

            if (!string.Equals(neutralLanguageCode, EnglishLanguageCode, StringComparison.OrdinalIgnoreCase))
                if (OverlayLanguageFile(neutralLanguageCode))
                    ActiveLanguageCode = neutralLanguageCode;

            if (!string.Equals(gameLanguageCode, neutralLanguageCode, StringComparison.OrdinalIgnoreCase))
                if (OverlayLanguageFile(gameLanguageCode))
                    ActiveLanguageCode = gameLanguageCode;

            initialized = true;

            if (string.Equals(gameLanguageCode, ActiveLanguageCode, StringComparison.OrdinalIgnoreCase))
                RepoAdminMenu.mls.LogInfo($"Loaded menu language '{ActiveLanguageCode}' with {activeStrings.Count} strings.");
            else if (string.Equals(neutralLanguageCode, ActiveLanguageCode, StringComparison.OrdinalIgnoreCase))
                RepoAdminMenu.mls.LogInfo($"Loaded menu language '{ActiveLanguageCode}' for game locale '{gameLanguageCode}' with {activeStrings.Count} strings.");
            else if (string.Equals(ActiveLanguageCode, EnglishLanguageCode, StringComparison.OrdinalIgnoreCase))
                RepoAdminMenu.mls.LogInfo($"No menu translation exists for game locale '{gameLanguageCode}'; using English ('{EnglishLanguageCode}') with {activeStrings.Count} strings.");
            else
                RepoAdminMenu.mls.LogInfo($"Loaded menu language '{ActiveLanguageCode}' for game locale '{gameLanguageCode}' with {activeStrings.Count} strings.");

            if (refreshMenu)
                Menu.RefreshLocalization();
        }

        internal static string Get(string identifier, string englishFallback) {
            if (string.IsNullOrEmpty(identifier))
                return englishFallback;

            if (activeStrings.TryGetValue(identifier, out string translatedText) && !string.IsNullOrEmpty(translatedText))
                return translatedText;

            if (englishStrings.TryGetValue(identifier, out string fallbackText) && !string.IsNullOrEmpty(fallbackText))
                return fallbackText;

            return englishFallback;
        }

        internal static string Format(string identifier, string englishFormat, params object[] values) {
            return string.Format(CultureInfo.CurrentCulture, Get(identifier, englishFormat), values);
        }

        private static string GetGameLanguageCode() {
            try {
                string localeCode = LocalizationManager.instance?.ActiveLocale?.LocaleReference?.Identifier.Code;
                if (!string.IsNullOrWhiteSpace(localeCode))
                    return localeCode.Replace('_', '-');
            } catch (Exception exception) {
                RepoAdminMenu.mls.LogWarning($"Could not read the active game locale: {exception.Message}");
            }
            return EnglishLanguageCode;
        }

        private static Dictionary<string, string> LoadLanguageFile(string languageCode, bool required) {
            string path = Path.Combine(LanguageDirectory, languageCode + ".json");
            if (!File.Exists(path)) {
                if (required)
                    RepoAdminMenu.mls.LogWarning($"Language file not found: {path}");
                return new Dictionary<string, string>(StringComparer.Ordinal);
            }

            try {
                string json = File.ReadAllText(path);
                Dictionary<string, string> strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return strings != null
                    ? new Dictionary<string, string>(strings, StringComparer.Ordinal)
                    : new Dictionary<string, string>(StringComparer.Ordinal);
            } catch (Exception exception) {
                RepoAdminMenu.mls.LogError($"Could not load language file '{path}': {exception}");
                return new Dictionary<string, string>(StringComparer.Ordinal);
            }
        }

        private static bool OverlayLanguageFile(string languageCode) {
            string path = Path.Combine(LanguageDirectory, languageCode + ".json");
            if (!File.Exists(path))
                return false;

            Dictionary<string, string> overlay = LoadLanguageFile(languageCode, false);
            foreach (KeyValuePair<string, string> entry in overlay)
                activeStrings[entry.Key] = entry.Value;

            return true;
        }
    }
}
