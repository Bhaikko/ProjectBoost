using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.SceneManagement {
    public class MusicPlayer : MonoBehaviour
    {

        void Awake() {
            DontDestroyOnLoad(this);
        }
    }
}

