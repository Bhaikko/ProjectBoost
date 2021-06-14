using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class SwordFish : MonoBehaviour, IEnemy
    {
        [SerializeField] float chargeSpeed = 10.0f;
        [SerializeField] float rotationSpeed = 10.0f;
        [SerializeField] float surpriseTime = 1.0f;
        [SerializeField] float rayCastDistance = 10.0f;

        [SerializeField] GameObject detectPrefab = null;

        private bool shouldRotate = false;
        private Vector3 targetRotation;
        private Vector3 velocityDirection;

        private FieldOfView fieldOfView = null;
        private bool isPlayerSpotted = false;
        private Vector3 lastPlayerPosition;

        private Rigidbody m_rigidbody;
        private Animator m_animator;
        private Animation detectAnimation;


        private bool didReact = false;

        private void Start() {
            m_rigidbody = GetComponent<Rigidbody>();

            fieldOfView = GetComponent<FieldOfView>();
            fieldOfView.SetOwnerRef(this);

            m_animator = GetComponentInChildren<Animator>();

            if (!detectPrefab) {
                Debug.Log("No Detection Prefab Reference Found.");
                return;
            }

            detectAnimation = detectPrefab.GetComponent<Animation>();
        }

        public void OnSeePlayer(Vector3 lastPlayerPosition)
        {
            if (!isPlayerSpotted) {
                isPlayerSpotted = true;
                this.lastPlayerPosition = lastPlayerPosition;
                StartCoroutine(AttackPlayer());
            }
        }

        private IEnumerator AttackPlayer() {
            m_animator.SetTrigger("Attack");
            detectAnimation.Play();
            yield return new WaitForSeconds(surpriseTime);
            velocityDirection = (lastPlayerPosition - transform.position).normalized;
        }

        public void PlayerHidden()
        {
            // isPlayerSpotted = false;
        }

        private void ChargeTowardsPlayer()
        {
            m_rigidbody.velocity += new Vector3(velocityDirection.x, 0.0f, 0.0f) * Time.deltaTime * chargeSpeed;
        }

        private void TurnAround(Vector3 target) {
            target -= transform.position;

            transform.localRotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.normalized), 
                Time.deltaTime * rotationSpeed
            );

            if (Vector3.Angle(target, transform.forward) <= Mathf.Epsilon) {
                shouldRotate = false;
            }
        }

        private void OnCollisionEnter(Collision collider) {
            isPlayerSpotted = false;
            m_rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            shouldRotate = true;
            targetRotation = transform.position - new Vector3(transform.forward.x, 0.0f, 0.0f);

        }

        private void Update() {
            if (isPlayerSpotted) {
                ChargeTowardsPlayer();
            }

            if (shouldRotate) {
                TurnAround(targetRotation);
            }
        }
    }
}
