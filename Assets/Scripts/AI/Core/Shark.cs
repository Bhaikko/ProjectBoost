using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.AI {
    public enum SharkState {
        PATROLLING,
        INVESTIGATING,
        ATTACKING
    };

    public class Shark : MonoBehaviour, IEnemy
    {
        [SerializeField] float detectRadius = 5.0f;
        [SerializeField] float surpriseTime = 1.0f;

        bool isPlayerSpotted = false;
        bool didReact = false;
        bool bStop = false;
        Vector3 lastSeenPosition;
        Vector3 originalPosition;

        [SerializeField] List<Waypoint> patrolPoints;
        int currentPatrolIndex = 0;

        private Diver diverRef = null;
        private PathFinder pathFinder = null;
        private FieldOfView fieldOfView = null;

        SharkState sharkState = SharkState.PATROLLING;

        Animator m_animator = null;

        private void Start() {
            diverRef = FindObjectOfType<Diver>();
            pathFinder = GetComponent<PathFinder>();
            fieldOfView = GetComponent<FieldOfView>();
            m_animator = GetComponentInChildren<Animator>();

            fieldOfView.SetOwnerRef(this);

            originalPosition = transform.position;

            StartCoroutine(ProcessBehavior());

        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }

        private float CalculateDistanceFromPlayer() {
            return Vector3.Distance(transform.position, diverRef.transform.position);
        }
        
        private void Patrol() {
            Vector3 currentPatrolPoint = patrolPoints[currentPatrolIndex].transform.position;

            pathFinder.MoveToDestination(currentPatrolPoint);

            if ((currentPatrolPoint - transform.position).magnitude <= Mathf.Epsilon) {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }

        }

        private void DecideBehavior() {
            // Debug.Log(diverRef.IsHiding());

            if (isPlayerSpotted) {
                if (diverRef.IsHiding()) {
                    isPlayerSpotted = false;
                    lastSeenPosition = diverRef.GetLastHidingPosition();
                    sharkState = SharkState.INVESTIGATING;

                } else {
                    sharkState = SharkState.ATTACKING;
                }
            } else {
                if (sharkState == SharkState.ATTACKING) {
                    sharkState = SharkState.INVESTIGATING;
                } 

                if (sharkState == SharkState.INVESTIGATING) {
                    if (Vector3.Distance(transform.position, lastSeenPosition) <= Mathf.Epsilon) {
                        sharkState = SharkState.PATROLLING;
                    }
                }
            }
        }

        private IEnumerator ProcessBehavior() {
            while (true) {
                switch (sharkState) {
                    case SharkState.ATTACKING:
                        if (bStop) {
                            yield return new WaitForSeconds(surpriseTime);
                            bStop = false;
                        }
                        pathFinder.MoveToDestination(lastSeenPosition);
                        break;

                    case SharkState.INVESTIGATING:
                        pathFinder.MoveToDestination(lastSeenPosition);
                        break;
                    case SharkState.PATROLLING:
                        Patrol();
                        break;

                    default:
                        break;
                }

                yield return null;
            }
        }

        private void Update() {
            DecideBehavior();
            // ProcessBehavior();
        }

        public void OnSeePlayer(Vector3 lastPlayerPosition)
        {
            isPlayerSpotted = true;
            this.lastSeenPosition = lastPlayerPosition;

            if (!didReact) {
                m_animator.SetTrigger("Detect");
                didReact = true;
                bStop = true;
            }

        }

        public void PlayerHidden()
        {
            didReact = false;
            isPlayerSpotted = false;
        }

        private void OnCollisionEnter(Collision collision) {
            Diver diver = collision.gameObject.GetComponent<Diver>();
            Debug.Log(diver.name);
            if (diver) {
                m_animator.SetTrigger("Attack");
            }
        }
    }
}
