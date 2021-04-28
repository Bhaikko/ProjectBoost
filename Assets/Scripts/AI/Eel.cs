using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class Eel : MonoBehaviour
    {
        [SerializeField] GameObject bodyRef;
        
        private float emissiveIntensity = -10.0f;
        [SerializeField] private float emissionChangeSpeed = 1.0f;

        private Material bodyMaterial;
        [SerializeField] Color initialEmissionColor;
        private float newIntensity;

        private void Start() {
            bodyMaterial = bodyRef.GetComponent<Renderer>().materials[0];

            newIntensity = emissiveIntensity;

        }

        private void Update() {
            emissiveIntensity = Mathf.SmoothStep(emissiveIntensity, newIntensity, emissionChangeSpeed * Time.deltaTime);

            bodyMaterial.SetColor("_Emission", initialEmissionColor * Mathf.Pow(2,  emissiveIntensity / 2.0f));
            
        }

        public void ChangeEmissionIntensity(float value) {
            newIntensity = value;
        }
    }
}
