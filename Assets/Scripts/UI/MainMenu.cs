using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectBoost.UI 
{   
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject mainMenuUI;
        [SerializeField] GameObject settingsUI;
        [SerializeField] GameObject controlsUI;

        [SerializeField] Slider volumeSlider;

        private void Start() {
            AudioListener.volume = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        }

        public void StartGame() {
            SceneManager.LoadScene(1);
        }

        public void ExitGame() {
            Application.Quit();
        }

        public void DisableUI() {
            settingsUI.SetActive(false);
            mainMenuUI.SetActive(false);
            controlsUI.SetActive(false);
        }

        public void OnPressSettings() {
            DisableUI();
            settingsUI.SetActive(true);
        }

        public void OnPressBack() {
            DisableUI();
            mainMenuUI.SetActive(true);
        }

        public void OnPressControls() {
            DisableUI();
            controlsUI.SetActive(true);
        }

        public void OnVolumeChange() {
            AudioListener.volume = volumeSlider.value;
        }
    }    
}
