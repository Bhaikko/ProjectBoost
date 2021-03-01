using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class JellyFish : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 10.0f;

        [SerializeField]
        Vector3 movementDirection = new Vector3(1.0f, 1.0f, 0.0f);

        Rigidbody m_rigidbody;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            movementDirection.Normalize();

            m_rigidbody.velocity = movementDirection * movementSpeed;
        }

        private void ChangeDirection(Collision collision)
        {
            Vector3 Normal = collision.contacts[0].normal;

            movementDirection = Vector3.Reflect(movementDirection.normalized, Normal);

        }

        private void OnCollisionEnter(Collision collision)
        {
            ChangeDirection(collision);
            m_rigidbody.velocity = movementDirection * movementSpeed;
        }
    }
}
