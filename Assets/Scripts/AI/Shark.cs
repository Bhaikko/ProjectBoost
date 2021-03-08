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

    public class Shark : MonoBehaviour
    {
        [SerializeField] float detectRadius = 5.0f;

        Vector3 lastSeenPosition;
        Vector3 originalPosition;

        [SerializeField] List<Waypoint> patrolPoints;
        int currentPatrolIndex = 0;

        private Diver diverRef = null;
        private PathFinder pathFinder = null;

        SharkState sharkState = SharkState.PATROLLING;

        private void Start() {
            diverRef = FindObjectOfType<Diver>();
            pathFinder = GetComponent<PathFinder>();

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

        }

        private void ProcessBehavior() {
            switch (sharkState) {
                case SharkState.ATTACKING:

                    break;

                case SharkState.INVESTIGATING:

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
    }
}
