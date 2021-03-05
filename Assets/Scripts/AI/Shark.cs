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
        [SerializeField] float moveSpeed = 5.0f;
        [SerializeField] float rotationSpeed = 5.0f;

        [SerializeField] float rayCastDistanceForPathfinding = 10.0f;
        [SerializeField] Vector3 offsetWhileNavigating = new Vector3(0.0f, 2.0f, 0.0f);

        Transform target = null;
        Vector3 targetPosition = new Vector3();
        Vector3 lastSeenPosition;
        Vector3 originalPosition;

        [SerializeField] List<Waypoint> patrolPoints;
        int currentPatrolIndex = 0;

        bool wasDetected = false;
        bool wasOccluded = false;

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

        private bool RayCastToPlayer() {
            Vector3 playerDirectionVector = diverRef.transform.position - transform.position;

            RaycastHit hit;
            bool isOccludingPath = Physics.Raycast(
                transform.position,
                playerDirectionVector.normalized,
                out hit,
                detectRadius
            );

            // Debug.DrawRay(transform.position, playerDirectionVector.normalized * 100.0f, Color.red);

            return hit.collider.GetComponent<Diver>() != null || hit.collider.GetComponentInParent<Diver>() != null;
        }

        private void Patrol() {
            Vector3 currentPatrolPoint = patrolPoints[currentPatrolIndex].transform.position;

            // MoveTowardsDestination(currentPatrolPoint);
            pathFinder.MoveToDestination(currentPatrolPoint);

            if ((currentPatrolPoint - transform.position).magnitude <= Mathf.Epsilon) {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }

        }

        private void Update() {
            // if (wasDetected) {
            //     if (diverRef.IsHiding()) {
            //         lastSeenPosition = diverRef.GetLastHidingPosition();
            //     } else {
            //         lastSeenPosition = diverRef.transform.position;
            //     }

            //     pathFinder.MoveTowardsDestination(lastSeenPosition);

            //     if ((lastSeenPosition - transform.position).magnitude <= Mathf.Epsilon) {
            //         wasDetected = false;
            //     }
                
            // } else if (CalculateDistanceFromPlayer() <= detectRadius) {
            //     if (RayCastToPlayer()) {
            //         wasDetected = true;
            //         lastSeenPosition = diverRef.transform.position;

            //         MoveTowardsDestination(diverRef.transform.position);
            //     } else {
            //         Patrol();
            //     }
            // }  else {
            //     Patrol();
            // }

            Patrol();
        }
    }
}
