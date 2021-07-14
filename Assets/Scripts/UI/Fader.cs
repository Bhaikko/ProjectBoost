using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.UI 
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] GameObject faderObject;
        [SerializeField] float fadingSpeed = 1.0f;

        private Material bodyMaterial;

        float faderValue;
        [SerializeField] bool shouldFadeIn = false;
        [SerializeField] bool shouldFadeOut = false;

        void Start()
        {
            bodyMaterial = faderObject.GetComponent<Renderer>().materials[0];
        }

        void Update()
        {
            bodyMaterial.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, faderValue));

            if (shouldFadeIn) {
                faderValue += fadingSpeed * Time.deltaTime;
                if (faderValue > 1.0f) {
                    faderValue = 1.0f;
                    shouldFadeIn = false;
                }
            }

            if (shouldFadeOut) {
                faderValue -= fadingSpeed * Time.deltaTime;
                if (faderValue < 0.0f) {
                    faderValue = 0.0f;
                    shouldFadeOut = false;
                }
            }


        }

        public void FadeIn() {
            shouldFadeIn = true;
        }

        public void FadeOut() {
            shouldFadeOut = true;
        }
    }
}
