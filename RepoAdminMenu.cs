using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RepoAdminMenu.Utils;

namespace RepoAdminMenu {

    [BepInPlugin(mod_guid, mod_name, mod_version)]
    [BepInDependency("REPOLib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("nickklmao.menulib", BepInDependency.DependencyFlags.HardDependency)]
    public class RepoAdminMenu : BaseUnityPlugin {

        private const string mod_guid = "proferabg.REPO.RepoAdminMenu";
        private const string mod_name = "Repo Admin Menu";
        private const string mod_version = "1.0.6";

        private static RepoAdminMenu _plugin;

        public static Harmony harmony = new Harmony(mod_guid);

        public static ManualLogSource mls;

        public void Awake() {
            // Plugin startup logic
            if (_plugin == null) {
                _plugin = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(mod_name);

            Configuration.Init(Config);
            Settings.Init();

            harmony.PatchAll();

            mls.LogInfo($"R.A.M. ({mod_version}) has been allocated!");
        }

    }
    
}
