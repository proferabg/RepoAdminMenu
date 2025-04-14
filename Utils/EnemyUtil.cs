using RepoAdminMenu.Patches;
using REPOLib.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class EnemyUtil {

        private static SortedDictionary<string, EnemySetup> enemySetups = new SortedDictionary<string, EnemySetup>();
        private static SortedDictionary<string, EnemySetup> modEnemySetups = new SortedDictionary<string, EnemySetup>();

        public static void Init() {
            enemySetups.Clear();
            modEnemySetups.Clear();

            List<EnemySetup> setups = new List<EnemySetup>();
            setups.AddRange(EnemyDirector.instance.enemiesDifficulty1);
            setups.AddRange(EnemyDirector.instance.enemiesDifficulty2);
            setups.AddRange(EnemyDirector.instance.enemiesDifficulty3);

            foreach (EnemySetup setup in setups) {
                if (setup.name.StartsWith("Enemy Group"))
                    continue;

                string name = setup.name.Replace("Enemy - ", string.Empty);
                switch (name) {
                    case "Ceiling Eye":
                        addEnemySetupSafe("Peeper", setup);
                        break;
                    case "Thin Man":
                        addEnemySetupSafe("Shadow Child", setup);
                        break;
                    case "Gnome":
                        addEnemySetupSafe("Gnome", setup);
                        break;
                    case "Duck":
                        addEnemySetupSafe("Apex Predator", setup);
                        break;
                    case "Slow Mouth":
                        addEnemySetupSafe("Spewer", setup);
                        break;
                    case "Valuable Thrower":
                        addEnemySetupSafe("Rugrat", setup);
                        break;
                    case "Animal":
                        addEnemySetupSafe("Animal", setup);
                        break;
                    case "Upscream":
                        addEnemySetupSafe("Upscream", setup);
                        break;
                    case "Hidden":
                        addEnemySetupSafe("Hidden", setup);
                        break;
                    case "Tumbler":
                        addEnemySetupSafe("Chef", setup);
                        break;
                    case "Bowtie":
                        addEnemySetupSafe("Bowtie", setup);
                        break;
                    case "Floater":
                        addEnemySetupSafe("Mentalist", setup);
                        break;
                    case "Bang":
                        addEnemySetupSafe("Bang", setup);
                        break;
                    case "Head":
                        addEnemySetupSafe("Headman", setup);
                        break;
                    case "Robe":
                        addEnemySetupSafe("Robe", setup);
                        break;
                    case "Hunter":
                        addEnemySetupSafe("Huntsman", setup);
                        break;
                    case "Runner":
                        addEnemySetupSafe("Reaper", setup);
                        break;
                    case "Beamer":
                        addEnemySetupSafe("Clown", setup);
                        break;
                    case "Slow Walker":
                        addEnemySetupSafe("Trudge", setup);
                        break;
                    default:
                        addModEnemySetupSafe(name, setup);
                        break;

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
                    Menu.toggleMenu();
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
                GameObject obj = UnityEngine.Object.Instantiate(enemySetup.spawnObjects[0], position, Quaternion.identity);
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
