using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

namespace RepoAdminMenu.Utils {
    internal class NetworkUtil : IOnEventCallback {

        internal static NetworkUtil instance { get; private set; } = null;

        internal bool registered = false;

        internal string clientRandomString = string.Empty;

        private static System.Random random = new System.Random();

        internal const byte RAMEventCode = 180;


        public NetworkUtil() {
            instance = this;
            if (!registered) {
                RepoAdminMenu.mls.LogInfo("Registering network event callbacks");
                PhotonNetwork.AddCallbackTarget(this);
                registered = true;
            }
        }

        public static void Init() {
            if (instance == null) {
                new NetworkUtil();
            }
        }

        private static string randomString(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static void SendCommandSteamIDString(string command, string steamId, string value, ReceiverGroup receiverGroup) {
            PhotonNetwork.RaiseEvent(RAMEventCode, new object[] { command, steamId, value }, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
        }

        internal static void SendCommandString(string command, string value, ReceiverGroup receiverGroup) {
            PhotonNetwork.RaiseEvent(RAMEventCode, new object[] { command, value }, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
        }

        internal static void SendCommand(string command, ReceiverGroup receiverGroup) {
            PhotonNetwork.RaiseEvent(RAMEventCode, new object[] { command }, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent) {
            if (photonEvent.Code == RAMEventCode) {
                RepoAdminMenu.mls.LogInfo("Received RAM Network Event!");
                object[] args = (object[])photonEvent.CustomData;

                if (args.Length == 1) {
                    string cmd = args[0].ToString();

                    switch (cmd) {
                        case "NeedSettings":
                            Settings.UpdateClients();
                            break;
                        default:
                            RepoAdminMenu.mls.LogWarning("Unknown command(" + args.Length + "): " + cmd);
                            break;
                    }
                } else if (args.Length == 2) {
                    string cmd = args[0].ToString();
                    string payload = args[1].ToString();

                    switch(cmd) {
                        case "SettingsSync":
                            if (!SemiFunc.IsMasterClient()) {
                                RepoAdminMenu.mls.LogInfo("Syncing settings with Master Client");
                                Settings.loadFromJson(payload);
                            }
                            break;
                        default:
                            RepoAdminMenu.mls.LogWarning("Unknown command(" + args.Length + "): " + cmd);
                            break;
                    }
                } else if (args.Length == 3) {
                    string cmd = args[0].ToString();
                    string steamId = args[1].ToString();
                    PlayerAvatar player = SemiFunc.PlayerGetFromSteamID(steamId);
                    if (player == null) {
                        RepoAdminMenu.mls.LogWarning("Invalid player: " + steamId);
                        return;
                    }

                    switch (cmd) {
                        case "KickPlayer":
                            if (!SemiFunc.IsMasterClient() && PlayerAvatar.instance.steamID.Equals(steamId)) {
                                RepoAdminMenu.mls.LogInfo("Received kick request");
                                string clientSecret = args[2].ToString();
                                if (clientSecret.Length != 16) {
                                    RepoAdminMenu.mls.LogInfo("Sending secret to master client to confirm kick");
                                    clientRandomString = randomString(16);
                                    SendCommandSteamIDString("KickPlayerAck", steamId, clientRandomString, ReceiverGroup.MasterClient);
                                } else if (clientSecret.Equals(clientRandomString)) {
                                    RepoAdminMenu.mls.LogInfo("I was kicked :(");
                                    NetworkManager.instance.LeavePhotonRoom();
                                    clientRandomString = "";
                                }
                            }
                            break;
                        case "KickPlayerAck":
                            if (SemiFunc.IsMasterClient()) {
                                RepoAdminMenu.mls.LogInfo("Sending kick acknowledge: " + SemiFunc.PlayerGetName(player));
                                SendCommandSteamIDString("KickPlayer", steamId, args[2].ToString(), ReceiverGroup.Others);
                            }
                            break;
                        default:
                            RepoAdminMenu.mls.LogWarning("Unknown command(" + args.Length + "): " + cmd);
                            break;
                    }
                } else {
                    RepoAdminMenu.mls.LogWarning("Received event with incorrect argument count: " + args.Length);
                    return;
                }
            }
        }
    }
}
