using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectBoost.Player;

namespace ProjectBoost.Environment {
    public class HidingVolume : MonoBehaviour
    {
        Diver diverRef;

        [SerializeField] float hidingOffset = 5.0f;

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
                Debug.Log("Inside");
                diverRef.SetHidingStatus(true, hidingOffset);
            }
        }

        private void OnTriggerExit(Collider collision) 
        {
            if (
                collision.gameObject.GetComponent<Diver>() || 
                collision.gameObject.GetComponentInParent<Diver>()
            ) {
                diverRef.SetHidingStatus(false);
            }
        }

        
    }
}
