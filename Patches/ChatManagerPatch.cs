using HarmonyLib;

namespace RepoAdminMenu {

    [HarmonyPatch(typeof(ChatManager))]
    internal class ChatManagerPatch {

        [HarmonyPatch("MessageSend")]
        [HarmonyPrefix]
        private static bool MessageSend_Prefix(ChatManager __instance, ref string ___chatMessage) {
            // must be enabled and must be host or single player
            if (!Configuration.EnableCommands.Value || !SemiFunc.IsMasterClientOrSingleplayer()) {
                return true;
            }

            string text = ___chatMessage.Replace("<b>|</b>", string.Empty);

            string[] args = text.ToLower().Split(' ');

            switch (args[0]) {
                case "!menu":
                    Menu.toggleMenu();
                    return false;
                default:
                    return true;
            }
        }

    }
}
