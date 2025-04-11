using UnityEngine;

namespace RepoAdminMenu.Utils {
    internal class ExtractionPointUtil {

        public static void discoverNext() {
            if (!RoundDirector.instance.extractionPointActive) {
                foreach (GameObject component in RoundDirector.instance.extractionPointList) {
                    ExtractionPoint extractionPoint = component.GetComponent<ExtractionPoint>();
                    if (extractionPoint != null && extractionPoint.StateIs(ExtractionPoint.State.Idle)) {
                        extractionPoint.OnClick();
                    }
                }
            }
        }

        public static void complete() {
            if (RoundDirector.instance.extractionPointActive) {
                RoundDirector.instance.extractionPointCurrent.isCompletedRightAway = true;
                RoundDirector.instance.extractionPointCurrent.StateSet(ExtractionPoint.State.Extracting);
            }
        }
    }
}
