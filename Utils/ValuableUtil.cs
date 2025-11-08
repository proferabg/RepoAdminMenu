using REPOLib.Modules;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class ValuableUtil {

        private static SortedDictionary<string, PrefabRef> tinyValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> smallValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> mediumValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> bigValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> wideValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> tallValuables = new SortedDictionary<string, PrefabRef>();
        private static SortedDictionary<string, PrefabRef> veryTallValuables = new SortedDictionary<string, PrefabRef>();

        public static void Init() {
            tinyValuables.Clear();
            smallValuables.Clear();
            mediumValuables.Clear();
            bigValuables.Clear();
            wideValuables.Clear();
            tallValuables.Clear();
            veryTallValuables.Clear();

            foreach (LevelValuables levelValuables in RunManager.instance.levelCurrent.ValuablePresets) {
                foreach (PrefabRef valuable in levelValuables.tiny) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!tinyValuables.ContainsKey(name))
                        tinyValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.small) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!smallValuables.ContainsKey(name))
                        smallValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.medium) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!mediumValuables.ContainsKey(name))
                        mediumValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.big) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!bigValuables.ContainsKey(name))
                        bigValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.wide) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!wideValuables.ContainsKey(name))
                        wideValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.tall) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!tallValuables.ContainsKey(name))
                        tallValuables.Add(name, valuable);
                }

                foreach (PrefabRef valuable in levelValuables.veryTall) {
                    string name = valuable.PrefabName.Replace("Valuable ", string.Empty);
                    if (!veryTallValuables.ContainsKey(name))
                        veryTallValuables.Add(name, valuable);
                }
            }


        }

        public static SortedDictionary<string, PrefabRef> getTinyValuables() {
            return tinyValuables;
        }

        public static SortedDictionary<string, PrefabRef> getSmallValuables() {
            return smallValuables;
        }

        public static SortedDictionary<string, PrefabRef> getMediumValuables() {
            return mediumValuables;
        }

        public static SortedDictionary<string, PrefabRef> getBigValuables() {
            return bigValuables;
        }

        public static SortedDictionary<string, PrefabRef> getWideValuables() {
            return wideValuables;
        }

        public static SortedDictionary<string, PrefabRef> getTallValuables() {
            return tallValuables;
        }

        public static SortedDictionary<string, PrefabRef> getVeryTallValuables() {
            return veryTallValuables;
        }

        public static void spawnValuable(PrefabRef valuable) {
            Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;
            Valuables.SpawnValuable(valuable, position, Quaternion.identity);
        }
    }
}
