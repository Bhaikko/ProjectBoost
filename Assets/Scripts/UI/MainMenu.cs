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

        Animator mainMenuUIAnimator = null;
        Animator settingsUIAnimator = null;
        Animator controlsUIAnimator = null;

        string animatorStateKey = "ShouldTransitionOut";

        [SerializeField] Slider volumeSlider;

        private void Start() {
            mainMenuUIAnimator = mainMenuUI.GetComponent<Animator>();
            settingsUIAnimator = settingsUI.GetComponent<Animator>();
            controlsUIAnimator = controlsUI.GetComponent<Animator>();

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
            mainMenuUIAnimator.SetBool(animatorStateKey, true);
            settingsUIAnimator.SetBool(animatorStateKey, true);
            controlsUIAnimator.SetBool(animatorStateKey, true);
        }

        public void OnPressSettings() {
            DisableUI();
            settingsUIAnimator.SetBool(animatorStateKey, false);
        }

        public void OnPressBack() {
            DisableUI();
            mainMenuUIAnimator.SetBool(animatorStateKey, false);
        }

        public void OnPressControls() {
            DisableUI();
            controlsUIAnimator.SetBool(animatorStateKey, false);
        }

        public void OnVolumeChange() {
            AudioListener.volume = volumeSlider.value;
        }
    }    
}
