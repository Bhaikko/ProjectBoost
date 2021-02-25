using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectBoost.Environment;
using ProjectBoost.Player;

namespace ProjectBoost.Core {
    public class GameMode : MonoBehaviour
    {
        [SerializeField] LandingPad launchPad;
        [SerializeField] LandingPad landingPad;
        [SerializeField] Vector3 spawnOffset = new Vector3(0.5f, 0.5f, 0.0f);

        [SerializeField] Diver diverPrefab;

        private Diver diverRef;

        void Start() {
            if (!launchPad) {
                Debug.LogError("No Launch Pad Reference Set.");
                return;
            }

            if (!landingPad) {
                Debug.LogError("No Landing Pad Reference Set.");
                return;
            }

            if (!diverPrefab) {
                Debug.LogError("No Diver class Specified.");
                return;
            }

            diverRef = Instantiate<Diver>(
                diverPrefab,
                launchPad.transform.position + spawnOffset,
                Quaternion.identity
            );

        }

    }
}
