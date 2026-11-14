using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RepoAdminMenu.Patches {

    [HarmonyPatch]
    internal class EnemyAIFreezeControllerPatch {

        private static readonly Type[] controllerTypes = [
            typeof(EnemyAnimal),
            typeof(EnemyBang),
            typeof(EnemyBangDirector),
            typeof(EnemyBeamer),
            typeof(EnemyBirthdayBoy),
            typeof(EnemyBombThrower),
            typeof(EnemyBowtie),
            typeof(EnemyCeilingEye),
            typeof(EnemyDuck),
            typeof(EnemyElsa),
            typeof(EnemyFloater),
            typeof(EnemyGnome),
            typeof(EnemyGnomeDirector),
            typeof(EnemyHeadController),
            typeof(EnemyHeadGrabber),
            typeof(EnemyHeartHugger),
            typeof(EnemyHidden),
            typeof(EnemyHunter),
            typeof(EnemyOogly),
            typeof(EnemyRobe),
            typeof(EnemyRunner),
            typeof(EnemyShadow),
            typeof(EnemySlowMouth),
            typeof(EnemySlowWalker),
            typeof(EnemySnatcher),
            typeof(EnemySpinny),
            typeof(EnemyThinMan),
            typeof(EnemyTick),
            typeof(EnemyTricycle),
            typeof(EnemyTumbler),
            typeof(EnemyUpscream),
            typeof(EnemyValuableThrower),
            typeof(EnemyStateChase),
            typeof(EnemyStateChaseBegin),
            typeof(EnemyStateChaseEnd),
            typeof(EnemyStateChaseSlow),
            typeof(EnemyStateDespawn),
            typeof(EnemyStateInvestigate),
            typeof(EnemyStateLookUnder),
            typeof(EnemyStateRoaming),
            typeof(EnemyStateSneak),
            typeof(EnemyStateSpawn),
            typeof(EnemyStateStunned),
        ];

        private static IEnumerable<MethodBase> TargetMethods() {
            foreach (Type controllerType in controllerTypes) {
                MethodInfo update = GetDeclaredParameterlessMethod(controllerType, "Update");
                if (update != null)
                    yield return update;

                MethodInfo fixedUpdate = GetDeclaredParameterlessMethod(controllerType, "FixedUpdate");
                if (fixedUpdate != null)
                    yield return fixedUpdate;
            }
        }

        private static MethodInfo GetDeclaredParameterlessMethod(Type type, string name) {
            return type.GetMethod(
                name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                null,
                Type.EmptyTypes,
                null
            );
        }

        [HarmonyPrefix]
        private static bool EnemyAIUpdate_Prefix() {
            return !Settings.instance.freezeEnemyAI;
        }
    }

    [HarmonyPatch(typeof(Enemy), "Update")]
    internal class EnemyAIFreezeMovementPatch {

        [HarmonyPrefix]
        private static bool Enemy_Update_Prefix(Enemy __instance) {
            if (!Settings.instance.freezeEnemyAI)
                return true;

            if (__instance.FreezeTimer < 0.1f)
                __instance.FreezeTimer = 0.1f;

            if (__instance.HasNavMeshAgent && __instance.NavMeshAgent != null)
                __instance.NavMeshAgent.Disable(0.1f);

            return false;
        }
    }

    [HarmonyPatch(typeof(EnemyNavMeshAgent), "Update")]
    internal class EnemyAIFreezeNavMeshPatch {

        [HarmonyPrefix]
        private static bool EnemyNavMeshAgent_Update_Prefix(EnemyNavMeshAgent __instance) {
            if (!Settings.instance.freezeEnemyAI)
                return true;

            __instance.Disable(0.1f);
            return false;
        }
    }
}
