using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment {
    public class Plant : MonoBehaviour
    {
        List<Material> plantMaterials = new List<Material>();

        private float plantTransparency = 1.0f;
        private float newTransparency;
        [SerializeField] float intensityChangeSpeed = 1.0f;

        void Start()
        {
            newTransparency = plantTransparency;
            foreach(Transform child in transform)
            {
                plantMaterials.Add(child.GetComponent<Renderer>().materials[0]);
            }
        }

        private void Update()
        {
            plantTransparency = Mathf.SmoothStep(plantTransparency, newTransparency, intensityChangeSpeed * Time.deltaTime);
            ChangeIntensities();
        }

        public void SetIntensities(float transparency)
        {
            this.newTransparency = transparency;
        }

        private void ChangeIntensities()
        {
            foreach (Material mat in plantMaterials) {
                mat.SetFloat("_Transparency", plantTransparency); 
            }
        }
    }
}
