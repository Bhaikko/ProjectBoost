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
            }
        }

        private void LightUpEels() {
            foreach (Eel eel in eels) {
                eel.ChangeEmissionIntensity(5.0f);
            }
        }

        private void UnLitEels() {
            foreach (Eel eel in eels) {
                eel.ChangeEmissionIntensity(-10.0f);
            }
        }
        
    }
}
