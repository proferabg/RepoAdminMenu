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
                string name = item.name.Replace("Item ", string.Empty);
                if (!items.ContainsKey(name))
                    items.Add(name, item);
            }
        }

        public static SortedDictionary<string, Item> getItems() {
            return items;
        }

        public static void spawnItem(Item item) {
            Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;
            GameObject spawnedObject;
            if (SemiFunc.IsMultiplayer()) {
                spawnedObject = Items.SpawnItem(item, position, Quaternion.identity);
            } else {
                spawnedObject = Object.Instantiate(item.prefab, position, Quaternion.identity);
            }
            ItemBattery itemBattery = spawnedObject.GetComponentInParent<ItemBattery>();
            if (itemBattery && itemBattery.batteryLifeInt < 6) {
                itemBattery.SetBatteryLife(100);
            }
        }
    }
}
