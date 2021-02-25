using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment {
    enum PadType {
        LAUNCH,
        LANDING,
        FRIENDLY
    }

    public class LandingPad : MonoBehaviour
    {
        [SerializeField] PadType padType = PadType.LAUNCH;


    }
}
