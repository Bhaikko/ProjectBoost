using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment {
    public class Plant : MonoBehaviour
    {
        List<Material> plantMaterials = new List<Material>();

        private float plantIntensity = 1.0f;
        [SerializeField] float intensityChangeSpeed = 1.0f;

        void Start()
        {
            foreach(Transform child in transform)
            {
                plantMaterials.Add(child.GetComponent<Renderer>().materials[0]);
            }
        }

        public void SetIntensities(float transparency)
        {
            foreach (Material mat in plantMaterials) {
                mat.SetFloat("_Transparency", transparency); 
            }
        }
    }
}
