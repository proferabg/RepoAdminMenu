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
                        enemySetups.Add("Peeper", setup);
                        break;
                    case "Thin Man":
                        enemySetups.Add("Shadow Child", setup);
                        break;
                    case "Gnome":
                        enemySetups.Add("Gnome", setup);
                        break;
                    case "Duck":
                        enemySetups.Add("Apex Predator", setup);
                        break;
                    case "Slow Mouth":
                        enemySetups.Add("Spewer", setup);
                        break;
                    case "Valuable Thrower":
                        enemySetups.Add("Rugrat", setup);
                        break;
                    case "Animal":
                        enemySetups.Add("Animal", setup);
                        break;
                    case "Upscream":
                        enemySetups.Add("Upscream", setup);
                        break;
                    case "Hidden":
                        enemySetups.Add("Hidden", setup);
                        break;
                    case "Tumbler":
                        enemySetups.Add("Chef", setup);
                        break;
                    case "Bowtie":
                        enemySetups.Add("Bowtie", setup);
                        break;
                    case "Floater":
                        enemySetups.Add("Mentalist", setup);
                        break;
                    case "Bang":
                        enemySetups.Add("Bang", setup);
                        break;
                    case "Head":
                        enemySetups.Add("Headman", setup);
                        break;
                    case "Robe":
                        enemySetups.Add("Robe", setup);
                        break;
                    case "Hunter":
                        enemySetups.Add("Huntsman", setup);
                        break;
                    case "Runner":
                        enemySetups.Add("Reaper", setup);
                        break;
                    case "Beamer":
                        enemySetups.Add("Clown", setup);
                        break;
                    case "Slow Walker":
                        enemySetups.Add("Trudge", setup);
                        break;
                    default:
                        modEnemySetups.Add(name, setup);
                        break;

                }
            }
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
