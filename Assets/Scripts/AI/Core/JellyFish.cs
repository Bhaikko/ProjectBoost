using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class JellyFish : MonoBehaviour
    {
        [SerializeField] float accelerationMagnitude = 10.0f;

        Vector3 accelrationDirection = new Vector3(1.0f, 1.0f, 0.0f);

        [SerializeField] float delayBetweenImpulses = 1.0f;

        Coroutine PeriodicImpulseEnumarator = null;

        Rigidbody m_rigidbody;
        Animator m_animator;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_animator = GetComponentInChildren<Animator>();
            
            accelrationDirection.Normalize();

            m_rigidbody.velocity = accelrationDirection * accelerationMagnitude;

            PeriodicImpulseEnumarator = StartCoroutine(PeriodicImpulse());
            
        }

        private void ChangeDirection(Collision collision)
        {
            Vector3 Normal = collision.contacts[0].normal;
            accelrationDirection = Vector3.Reflect(accelrationDirection.normalized, Normal);
            accelrationDirection.y = Mathf.Abs(accelrationDirection.y);
            
        }

        private void ApplyImpusle(float factor = 1.0f) {
            m_animator.SetTrigger("Accelerate");

            Vector3 amount = accelrationDirection * accelerationMagnitude * factor;
            amount.y *= 2.5f;
            m_rigidbody.velocity += amount;
        }

        private void OnCollisionEnter(Collision collision)
        {
            StopCoroutine(PeriodicImpulseEnumarator);
            ChangeDirection(collision);
            ApplyImpusle(1.2f);
            
            PeriodicImpulseEnumarator = StartCoroutine(PeriodicImpulse());

        }

        private IEnumerator PeriodicImpulse() {
            while (true) {
                yield return new WaitForSeconds(delayBetweenImpulses);
                ApplyImpusle();
                
            }
        }
    }
}
