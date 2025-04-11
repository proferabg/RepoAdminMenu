using REPOLib.Modules;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class ValuableUtil {

        private static SortedDictionary<string, GameObject> tinyValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> smallValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> mediumValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> bigValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> wideValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> tallValuables = new SortedDictionary<string, GameObject>();
        private static SortedDictionary<string, GameObject> veryTallValuables = new SortedDictionary<string, GameObject>();

        public static void Init() {
            tinyValuables.Clear();
            smallValuables.Clear();
            mediumValuables.Clear();
            bigValuables.Clear();
            wideValuables.Clear();
            tallValuables.Clear();
            veryTallValuables.Clear();

            foreach (LevelValuables levelValuables in RunManager.instance.levelCurrent.ValuablePresets) {
                foreach (GameObject valuable in levelValuables.tiny) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!tinyValuables.ContainsKey(name))
                        tinyValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.small) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!smallValuables.ContainsKey(name))
                        smallValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.medium) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!mediumValuables.ContainsKey(name))
                        mediumValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.big) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!bigValuables.ContainsKey(name))
                        bigValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.wide) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!wideValuables.ContainsKey(name))
                        wideValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.tall) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!tallValuables.ContainsKey(name))
                        tallValuables.Add(name, valuable);
                }

                foreach (GameObject valuable in levelValuables.veryTall) {
                    string name = valuable.name.Replace("Valuable ", string.Empty);
                    if (!veryTallValuables.ContainsKey(name))
                        veryTallValuables.Add(name, valuable);
                }
            }


        }

        public static SortedDictionary<string, GameObject> getTinyValuables() {
            return tinyValuables;
        }

        public static SortedDictionary<string, GameObject> getSmallValuables() {
            return smallValuables;
        }

        public static SortedDictionary<string, GameObject> getMediumValuables() {
            return mediumValuables;
        }

        public static SortedDictionary<string, GameObject> getBigValuables() {
            return bigValuables;
        }

        public static SortedDictionary<string, GameObject> getWideValuables() {
            return wideValuables;
        }

        public static SortedDictionary<string, GameObject> getTallValuables() {
            return tallValuables;
        }

        public static SortedDictionary<string, GameObject> getVeryTallValuables() {
            return veryTallValuables;
        }

        public static void spawnValuable(GameObject valuable) {
            Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;
            if (SemiFunc.IsMultiplayer()) {
                if (!valuable.TryGetComponent(out ValuableObject valuableObject)) {
                    ItemInfoExtraUI.instance.ItemInfoText($"Could not spawn valuable '" + valuable.name + "'", Color.red);
                    return;
                }
                Valuables.SpawnValuable(valuableObject, position, Quaternion.identity);
            } else {
                UnityEngine.Object.Instantiate(valuable.gameObject, position, Quaternion.identity);
            }
        }
    }
}
