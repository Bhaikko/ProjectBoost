using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.AI {
    public class Shark : MonoBehaviour
    {
        [SerializeField] float detectRadius = 5.0f;
        [SerializeField] float moveSpeed = 5.0f;

        [SerializeField] float rayCastDistanceForPathfinding = 10.0f;
        [SerializeField] Vector3 offsetWhileNavigating = new Vector3(0.0f, 2.0f, 0.0f);

        Transform target = null;
        Vector3 targetPosition = new Vector3();
        Vector3 lastSeenPosition;
        Vector3 originalPosition;

        bool wasDetected = false;
        bool wasOccluded = false;

        private Diver diverRef = null;

        private void Start() {
            diverRef = FindObjectOfType<Diver>();

            originalPosition = transform.position;

        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }

        private void OnDrawGizmos() {
            if (diverRef) {
                Vector3 playerPositionVector = diverRef.transform.position - transform.position;
                
                // Gizmos.DrawLine(
                //     transform.position, 
                //     (new Vector3(playerPositionVector.x, 0.0f, 0.0f).normalized * 10.0f) + transform.position
                // );

                // Gizmos.DrawLine(
                //     transform.position, 
                //     (playerPositionVector.normalized * detectRadius) + transform.position
                // );

                // if (!target) {
                //     return;
                // }

                // Gizmos.DrawCube(targetPosition, new Vector3(1.0f, 1.0f, 1.0f));
            }

        }

        private float CalculateDistanceFromPlayer() {
            return Vector3.Distance(transform.position, diverRef.transform.position);
        }

        private void MoveTowardsDestination(Vector3 destination) {
            Vector3 playerPositionVector = destination - transform.position;

            RaycastHit hit;
            bool isOccludingPath = Physics.Raycast(
                transform.position,
                new Vector3(playerPositionVector.x, 0.0f, 0.0f).normalized,
                out hit,
                rayCastDistanceForPathfinding
            );

            if (!wasOccluded) {
                target = diverRef.transform;
                targetPosition = destination;
            }

            if (!wasOccluded && isOccludingPath) {
                if (hit.collider.GetComponent<OctopusHands>()) {
                    wasOccluded = true;
                    target = hit.collider.transform;

                    Vector3 obstacleDirection = target.gameObject.GetComponent<OctopusHands>().GetMovementDirection();

                    targetPosition = 
                                    hit.collider.transform.position + 
                                    obstacleDirection * target.localScale.y / 2.0f + 
                                    obstacleDirection.y * offsetWhileNavigating;

                    Vector3 direction = (targetPosition - transform.position).normalized;
                    Debug.Log(target.gameObject.GetComponent<OctopusHands>().GetMovementDirection());
                    targetPosition.x += direction.x * 2.0f;
                }

            }

            if ((targetPosition - transform.position).magnitude <= Mathf.Epsilon) {
                wasOccluded = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        private bool RayCastToPlayer() {
            Vector3 playerPositionVector = diverRef.transform.position - transform.position;

            RaycastHit hit;
            bool isOccludingPath = Physics.Raycast(
                transform.position,
                playerPositionVector.normalized,
                out hit,
                detectRadius
            );

            return hit.collider.GetComponent<Diver>() != null || hit.collider.GetComponentInParent<Diver>() != null;
        }

        private void Update() {
            if (CalculateDistanceFromPlayer() <= detectRadius) {
                // Debug.Log("Detecting");
                if (RayCastToPlayer()) {
                    wasDetected = true;
                    lastSeenPosition = diverRef.transform.position;

                    MoveTowardsDestination(diverRef.transform.position);
                } 
            } else if (wasDetected) {
                MoveTowardsDestination(lastSeenPosition);

                if ((lastSeenPosition - transform.position).magnitude <= Mathf.Epsilon) {
                    wasDetected = false;
                }
            } else {
                // Return to Patroling Position
                MoveTowardsDestination(originalPosition);

            }
        }
    }
}
