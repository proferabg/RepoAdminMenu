using RepoAdminMenu.Patches;
using REPOLib.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class EnemyUtil {

        private static readonly Dictionary<string, string> enemyDictionary = new Dictionary<string, string> {
                { "Animal", "Animal" },
                { "Bang", "Bang" },
                { "Beamer", "Clown" },
                { "Birthday Boy", "Birthday Boy" },
                { "Bomb Thrower", "Cleanup Crew" },
                { "Bowtie", "Bowtie" },
                { "Ceiling Eye", "Peeper" },
                { "Duck", "Apex Predator" },
                { "Elsa", "Elsa" },
                { "Floater", "Mentalist" },
                { "Gnome", "Gnome" },
                { "Head", "Headman" },
                { "Head Grabber", "Headgrab" },
                { "Heart Hugger", "Heart Hugger" },
                { "Hidden", "Hidden" },
                { "Hunter", "Huntsman" },
                { "Oogly", "Oogly" },
                { "Robe", "Robe" },
                { "Runner", "Reaper" },
                { "Shadow", "Loom" },
                { "Slow Mouth", "Spewer" },
                { "Slow Walker", "Trudge" },
                { "Spinny", "Gambit" },
                { "Thin Man", "Shadow Child" },
                { "Tick", "Tick" },
                { "Tricycle", "Bella" },
                { "Tumbler", "Chef" },
                { "Upscream", "Upscream" },
                { "Valuable Thrower", "Rugrat" },
            };

        private static SortedDictionary<string, EnemySetup> enemySetups = new SortedDictionary<string, EnemySetup>();
        private static SortedDictionary<string, EnemySetup> modEnemySetups = new SortedDictionary<string, EnemySetup>();

        public static void Init() {
            enemySetups.Clear();
            modEnemySetups.Clear();

            List<EnemySetup> setups =
            [
                .. EnemyDirector.instance.enemiesDifficulty1,
                .. EnemyDirector.instance.enemiesDifficulty2,
                .. EnemyDirector.instance.enemiesDifficulty3,
            ];

            foreach (EnemySetup setup in setups) {
                if (setup.name.StartsWith("Enemy Group"))
                    continue;

                string name = setup.name.Replace("Enemy - ", string.Empty);
                try {
                    if (enemyDictionary.TryGetValue(name, out var value))
                        addEnemySetupSafe(value, setup);
                    else addModEnemySetupSafe(name, setup);
                } catch {
                    RepoAdminMenu.mls.LogError($"Invalid enemy setup for name '{setup.name}'!");
                }
            }
        }

        internal static void addEnemySetupSafe(string name, EnemySetup setup) {
            if(!enemySetups.ContainsKey(name))
                enemySetups.Add(name, setup);
        }

        internal static void addModEnemySetupSafe(string name, EnemySetup setup) {
            if (!modEnemySetups.ContainsKey(name))
                modEnemySetups.Add(name, setup);
        }

        public static SortedDictionary<string, EnemySetup> getEnemies() {
            return enemySetups;
        }

        public static SortedDictionary<string, EnemySetup> getModEnemies() {
            return modEnemySetups;
        }


        private static EnemySetup getEnemySetup(string name) {
            EnemySetup enemy = enemySetups.GetValueOrDefault(name, null);
            if(enemy == null) {
                enemy = modEnemySetups.GetValueOrDefault(name, null);
            }
            return enemy;
        }

        public static void spawnEnemy(string enemy) {
            if (EnemyParentPatch.spawning) {
                MissionUI.instance.MissionText($"Already spawning enemy '" + enemy + "'!", Color.red, Color.red, 3f);
            } else {
                EnemySetup enemySetup = getEnemySetup(enemy);
                if (enemySetup != null) {
                    Vector3 position = PlayerAvatar.instance.transform.position;
                    EnemyDirector.instance.StartCoroutine(SpawnEnemy(enemySetup, position));
                    if (Configuration.CloseMenuOnSpawning.Value) {
                        Menu.toggleMenu();
                    }
                    MissionUI.instance.MissionText($"Spawning '{enemy}'. Run...!", Color.red, Color.red, 3f);
                } else {
                    MissionUI.instance.MissionText($"Could not find enemy named '" + enemy + "'", Color.red, Color.red, 3f);
                }
            }
        }

        private static IEnumerator SpawnEnemy(EnemySetup enemySetup, Vector3 position) {
            yield return new WaitForSeconds(3f);
            if (SemiFunc.IsMultiplayer()) {
                Enemies.SpawnEnemy(enemySetup, position, Quaternion.identity, spawnDespawned: false);
            } else {
                EnemyParentPatch.spawning = true;
                LevelGenerator.Instance.EnemiesSpawned = -1;
                GameObject obj = UnityEngine.Object.Instantiate(enemySetup.spawnObjects[0].Prefab, position, Quaternion.identity);
                EnemyParent parent = obj.GetComponent<EnemyParent>();
                if (parent != null) {
                    parent.SetupDone = true;
                    obj.GetComponentInChildren<Enemy>().EnemyTeleported(position);
                    EnemyDirector.instance.FirstSpawnPointAdd(parent);
                    EnemyDirector.instance.enemiesSpawned.Add(parent);
                    foreach (PlayerAvatar player in SemiFunc.PlayerGetAll()) {
                        parent.Enemy.PlayerAdded(player.photonView.ViewID);
                    }
                }
            }
        }

    }
}
