using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
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

        Animator animator;

        private void Start() {
            bodyMaterial = bodyRef.GetComponent<Renderer>().materials[0];
            animator = GetComponentInChildren<Animator>();

            newIntensity = emissiveIntensity;

        }

        private void Update() {
            emissiveIntensity = Mathf.SmoothStep(emissiveIntensity, newIntensity, emissionChangeSpeed * Time.deltaTime);

            bodyMaterial.SetColor("_Emission", initialEmissionColor * Mathf.Pow(2,  emissiveIntensity / 2.0f));
            
        }

        public void ChangeEmissionIntensity(float value) {
            newIntensity = value;
        }

        private void OnCollisionEnter(Collision collision) {
            Diver diver = collision.gameObject.GetComponent<Diver>();

            if (diver) {
                Vector3 diverDirection = diver.transform.position - transform.position;

                float cosAngle = Vector3.Dot(diverDirection.normalized, transform.right);

                if (cosAngle < 0.0f) {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Rad2Deg * Mathf.Acos(cosAngle) - 150.0f);
                    animator.SetTrigger("AttackBack");
                } else {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Rad2Deg * Mathf.Acos(cosAngle) - 30.0f);
                    animator.SetTrigger("AttackForward");
                }
            }
        }
    }
}
