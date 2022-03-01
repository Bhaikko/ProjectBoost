using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectBoost.Player;
using ProjectBoost.AI;

namespace ProjectBoost.Environment {
    public class HidingVolume : MonoBehaviour
    {
        Diver diverRef;

        [SerializeField] float hidingOffset = 5.0f;
        [SerializeField] List<Eel> eels = new List<Eel>();
        [SerializeField] PlantController plantController = null;

        void Start()
        {
            diverRef = FindObjectOfType<Diver>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (
                collision.gameObject.GetComponent<Diver>() || 
                collision.gameObject.GetComponentInParent<Diver>()
            ) {
                diverRef.SetHidingStatus(true, hidingOffset);
                LightUpEels();
                plantController.ChangeIntensitiesOfPlants(0.1f);
            }
        }

        private void OnTriggerExit(Collider collision) 
        {
            if (
                collision.gameObject.GetComponent<Diver>() || 
                collision.gameObject.GetComponentInParent<Diver>()
            ) {
                diverRef.SetHidingStatus(false);
                UnLitEels();
                plantController.ChangeIntensitiesOfPlants(1.0f);
                
            }
        }

        private void LightUpEels() {
            foreach (Eel eel in eels) {
                eel.ChangeEmissionIntensity(-1.0f);
            }
        }

        private void UnLitEels() {
            foreach (Eel eel in eels) {
                eel.ChangeEmissionIntensity(-10.0f);
            }
        }
        
    }
}
