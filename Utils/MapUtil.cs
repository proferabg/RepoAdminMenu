using System.Collections.Generic;

namespace RepoAdminMenu.Utils {
    internal class MapUtil {

        private static SortedDictionary<string, Level> maps = new SortedDictionary<string, Level>();
        private static Dictionary<Level, bool> mapToggles = new Dictionary<Level, bool>();

        private static Level nextLevel = null;


        public static void Init() {
            if (maps.Count > 0)
                return;

            foreach (Level level in RunManager.instance.levels) {
                maps.Add(level.name.Replace("Level - ", string.Empty), level);
                mapToggles.Add(level, true);
            }
        }

        public static SortedDictionary<string, Level> getMaps() {
            return maps;
        }

        public static Dictionary<Level, bool> getMapToggles() {
            return mapToggles;
        }

        public static void setMapEnabled(Level level, bool enabled) {
            if (mapToggles.ContainsKey(level)) {
                mapToggles[level] = enabled;
            }
        }

        public static bool isMapEnabled(Level level) {
            return mapToggles.GetValueOrDefault(level, false);
        }

        public static int getEnabledCount() {
            int count = 0;
            foreach (bool enabled in mapToggles.Values) {
                if (enabled) count++;
            }
            return count;
        }

        public static void changeLevel(Level level) {
            nextLevel = level;
            RunManager.instance.ChangeLevel(true, false, RunManager.ChangeLevelType.RunLevel);
        }

        public static Level getNextLevel() {
            return nextLevel;
        }

        public static void clearNextLevel() {
            nextLevel = null;
        }

        public static void resetLevels() {
            foreach (KeyValuePair<Level, bool> entry in mapToggles) {
                if (!isMapEnabled(entry.Key)) {
                    if (RunManager.instance.levels.Contains(entry.Key)) {
                        RunManager.instance.levels.Remove(entry.Key);
                    }
                } else {
                    if (!RunManager.instance.levels.Contains(entry.Key)) {
                        RunManager.instance.levels.Add(entry.Key);
                    }
                }
            }
        }
    }
}
