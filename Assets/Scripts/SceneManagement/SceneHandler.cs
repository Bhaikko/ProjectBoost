using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectBoost.SceneManagement {
    public class SceneHandler : MonoBehaviour
    {
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int totalScene = SceneManager.sceneCountInBuildSettings;

            SceneManager.LoadScene((currentSceneIndex + 1) % totalScene);
        }

        public void LoadCurrentScene() 
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentSceneIndex);
            
        }

        public void GameOver() 
        {
            int lastSceneIndex = SceneManager.sceneCountInBuildSettings;

            SceneManager.LoadScene(lastSceneIndex - 1);
        }

        
    }
}
