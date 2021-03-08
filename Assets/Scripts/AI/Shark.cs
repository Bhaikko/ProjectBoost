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

        bool isPlayerSpotted = false;
        Vector3 lastSeenPosition;
        Vector3 originalPosition;

        [SerializeField] List<Waypoint> patrolPoints;
        int currentPatrolIndex = 0;

        private Diver diverRef = null;
        private PathFinder pathFinder = null;
        private FieldOfView fieldOfView = null;

        SharkState sharkState = SharkState.PATROLLING;

        private void Start() {
            diverRef = FindObjectOfType<Diver>();
            pathFinder = GetComponent<PathFinder>();
            fieldOfView = GetComponent<FieldOfView>();

            fieldOfView.SetOwnerRef(this);

            originalPosition = transform.position;

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
            if (isPlayerSpotted) {
                sharkState = SharkState.ATTACKING;
            } else {
                if (sharkState == SharkState.ATTACKING) {
                    sharkState = SharkState.INVESTIGATING;
                } 

                if (sharkState == SharkState.INVESTIGATING) {
                    if (Vector3.Distance(transform.position, lastSeenPosition) <= Mathf.Epsilon) {
                        Debug.Log("Lost Target");
                        sharkState = SharkState.PATROLLING;
                    }
                }
            }
        }

        private void ProcessBehavior() {
            switch (sharkState) {
                case SharkState.ATTACKING:
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
        }

        private void Update() {
            DecideBehavior();
            ProcessBehavior();
        }

        public void OnSeePlayer(Vector3 lastPlayerPosition)
        {
            isPlayerSpotted = true;
            this.lastSeenPosition = lastPlayerPosition;
        }

        public void PlayerHidden()
        {
            isPlayerSpotted = false;
        }
    }
}
