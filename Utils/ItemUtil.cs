using REPOLib.Extensions;
using REPOLib.Modules;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class ItemUtil {

        private static SortedDictionary<string, Item> items = new SortedDictionary<string, Item>();

        public static void Init() {
            items.Clear();
            foreach (Item item in StatsManager.instance.GetItems()) {
                items.Add(item.name.Replace("Item ", string.Empty), item);
            }
        }

        public static SortedDictionary<string, Item> getItems() {
            return items;
        }

        public static void spawnItem(Item item) {
            Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;
            if (SemiFunc.IsMultiplayer()) {
                Items.SpawnItem(item, position, Quaternion.identity);
            } else {
                Object.Instantiate(item.prefab, position, Quaternion.identity);
            }
        }
    }
}
