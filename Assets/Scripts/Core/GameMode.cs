using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectBoost.Environment;
using ProjectBoost.Player;
using ProjectBoost.SceneManagement;

namespace ProjectBoost.Core {
    public class GameMode : MonoBehaviour
    {
        private SceneHandler sceneHandler;

        void Start() {
            
            sceneHandler = FindObjectOfType<SceneHandler>();

        }

        public void HandleDeath() {
            sceneHandler.LoadCurrentScene();
        }

        public void LoadNextScene() {
            sceneHandler.LoadNextScene();
        }
        

    }
}
