using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class CosmeticUtil {

        public static void spawnCosmetic(SemiFunc.Rarity rarity) {
            ValuableDirector director = ValuableDirector.instance;

            if (director == null || director.cosmeticWorldObjectSetups == null) {
                ItemInfoExtraUI.instance.ItemInfoText(LanguageUtil.Get("message.cosmetic_unavailable", "Cosmetic boxes are not available in this level."), Color.red);
                return;
            }

            ValuableDirector.CosmeticWorldObjectSetup setup = null;
            int index = (int)rarity;

            if (index >= 0 && index < director.cosmeticWorldObjectSetups.Count &&
                director.cosmeticWorldObjectSetups[index].rarity == rarity) {
                setup = director.cosmeticWorldObjectSetups[index];
            } else {
                foreach (var candidate in director.cosmeticWorldObjectSetups) {
                    if (candidate.rarity == rarity) {
                        setup = candidate;
                        break;
                    }
                }
            }

            if (setup == null || setup.prefab == null) {
                ItemInfoExtraUI.instance.ItemInfoText(LanguageUtil.Format("message.cosmetic_rarity_missing", "No cosmetic box exists for rarity '{0}'.", LanguageUtil.Get("rarity." + rarity.ToString().ToLowerInvariant(), rarity.ToString())), Color.red);
                return;
            }


            Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;

            int spawned = 0;

            for (int i = 0; i < Settings.instance.spawnerCount; i++) {
                GameObject result;

                if (SemiFunc.IsMultiplayer()) {
                    result = PhotonNetwork.InstantiateRoomObject(setup.prefab.ResourcePath, position, Quaternion.identity, 0, null);
                } else {
                    result = UnityEngine.Object.Instantiate(setup.prefab.Prefab, position, Quaternion.identity);
                }

                if (result != null) {
                    spawned++;
                }
            }

            MissionUI.instance.MissionText(LanguageUtil.Format("message.cosmetic_spawned", "Spawned {0} {1} cosmetic box(es).", spawned, LanguageUtil.Get("rarity." + rarity.ToString().ToLowerInvariant(), rarity.ToString())), Color.black, Color.black, 3f);
        }
    }
}
