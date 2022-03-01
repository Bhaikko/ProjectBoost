using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment {
    public class PlantController : MonoBehaviour
    {
        List<Plant> plantRefs = new List<Plant>();

        // Start is called before the first frame update
        void Start()
        {
            foreach(Transform child in transform)
            {
                Plant plant = child.GetComponent<Plant>();
                if (plant) {
                    plantRefs.Add(child.GetComponent<Plant>());
                }
            }
        }

        public void ChangeIntensitiesOfPlants(float newIntensity)
        {
            foreach (Plant plant in plantRefs) {
                plant.SetIntensities(newIntensity);
            }
        }
    }
}