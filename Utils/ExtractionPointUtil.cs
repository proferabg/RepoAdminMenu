using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;

namespace RepoAdminMenu.Utils {
    internal class ExtractionPointUtil {

        public static void discoverNext() {
            if (!ReflectionUtil.GetFieldValue<bool>(RoundDirector.instance, "extractionPointActive")) {
                var list = ReflectionUtil.GetFieldValue<List<GameObject>>(RoundDirector.instance, "extractionPointList");
                foreach (GameObject component in list) {
                    ExtractionPoint extractionPoint = component.GetComponent<ExtractionPoint>();
                    if (extractionPoint != null && (int)ReflectionUtil.GetFieldValue<object>(extractionPoint, "currentState") == 1) {
                        extractionPoint.OnClick();
                    }
                }
            }
        }

        public static void complete() {
            if (ReflectionUtil.GetFieldValue<bool>(RoundDirector.instance, "extractionPointActive")) {
                var epCurrent = ReflectionUtil.GetFieldValue<ExtractionPoint>(RoundDirector.instance, "extractionPointCurrent");
                ReflectionUtil.SetFieldValue(epCurrent, "isCompletedRightAway", true);
                ReflectionUtil.SetFieldValue(epCurrent, "currentState", 3);
            }
        }
    }
}
