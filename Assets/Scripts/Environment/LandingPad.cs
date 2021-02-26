using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment {

    public class LandingPad : MonoBehaviour
    {
        public enum PadType {
            LAUNCH,
            LANDING,
            FRIENDLY
        }

        [SerializeField] PadType padType = PadType.LAUNCH;

        public PadType GetPadType() {
            return this.padType;
        }

    }
}
