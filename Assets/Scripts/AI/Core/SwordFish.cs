using System.Collections;
using System.Collections.Generic;
using ProjectBoost.AI.Components;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.AI {
    public class SwordFish : MonoBehaviour, IEnemy
    {
        [SerializeField] float chargeSpeed = 10.0f;
        [SerializeField] float rotationSpeed = 10.0f;
        [SerializeField] float surpriseTime = 1.0f;

        private bool shouldRotate = false;
        private Vector3 targetRotation;
        private float velocityDirection;

        // Components
        private FieldOfView fieldOfView = null;
        private Detection detection = null;

        private bool isPlayerSpotted = false;

        private Rigidbody m_rigidbody;
        private Animator m_animator;
        private KillAttach killAttach;

        private bool didReact = false;

        private void Start() {
            m_rigidbody = GetComponent<Rigidbody>();

            fieldOfView = GetComponent<FieldOfView>();
            fieldOfView.SetOwnerRef(this);

            detection = GetComponentInChildren<Detection>();
            killAttach = GetComponent<KillAttach>();

            m_animator = GetComponentInChildren<Animator>();
        }

        public void OnSeePlayer(Vector3 lastPlayerPosition)
        {
            if (!isPlayerSpotted && !didReact) {
                StartCoroutine(AttackPlayer(lastPlayerPosition));

                didReact = true;
            }
        }

        private IEnumerator AttackPlayer(Vector3 lastPlayerPosition) {
            m_animator.SetTrigger("Attack");
            detection.PlayAnimation();
            yield return new WaitForSeconds(surpriseTime);

            velocityDirection = (lastPlayerPosition - transform.position).normalized.x;
            velocityDirection = velocityDirection > 0.0f ? 1.0f : -1.0f;

            isPlayerSpotted = true;
        }

        public void PlayerHidden()
        {
            didReact = false;
        }

        private void ChargeTowardsPlayer()
        {
            m_rigidbody.velocity += new Vector3(velocityDirection, 0.0f, 0.0f) * Time.deltaTime * chargeSpeed;
            
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
            if (collider.gameObject.GetComponent<Diver>()) {
                killAttach.AttachBoneToPlayer();
                return;
            }
            
            isPlayerSpotted = false;
            didReact = false;
            m_rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            detection.Flip();
            m_animator.SetTrigger("StopAttack");

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
