using HarmonyLib;
using RepoAdminMenu.Utils;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch(typeof(NetworkConnect))]
    internal class NetworkConnectPatch {

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Awake_Postfix() {
            NetworkUtil.Init();
        }

        [HarmonyPatch("OnCreatedRoom")]
        [HarmonyPostfix]
        private static void OnCreatedRoom_Postfix() {
            if (Configuration.ResetSettingsOnLobbyCreation.Value) {
                RepoAdminMenu.mls.LogInfo("Resetting settings");
                Settings.instance = new Settings();
            }
        }

        [HarmonyPatch("OnJoinedRoom")]
        [HarmonyPostfix]
        private static void OnJoinedRoom_Postfix() {
            NetworkUtil.instance.clientRandomString = string.Empty;
            NetworkUtil.SendCommand("NeedSettings", Photon.Realtime.ReceiverGroup.MasterClient);
        }

    }
}
