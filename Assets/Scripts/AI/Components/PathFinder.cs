using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 10.0f;
        [SerializeField] float movementSpeed = 10.0f;

        [SerializeField] float maxRayCastDistanceForPathfinding = 10.0f;
        [SerializeField] float offsetWhileNavigating = 2.0f;

        private void RotateTowards(Vector3 target) {
            target -= transform.position;

            transform.localRotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.normalized), 
                Time.deltaTime * rotationSpeed
            );
        }

        public void MoveToDestination(Vector3 destination)
        {
            RaycastHit hit;
            bool isPathOcculuded = Physics.Raycast(
                transform.position,
                new Vector3(transform.forward.x, 0.0f, 0.0f),
                // transform.forward,
                out hit,
                maxRayCastDistanceForPathfinding
            );

            if (
                isPathOcculuded &&
                Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, hit.collider.transform.position)
            ) {
                if (hit.collider.GetComponent<OctopusHands>()) { 
                    Transform target = hit.collider.transform;

                    Vector3 obstacleDirection = target.gameObject.GetComponent<OctopusHands>().GetMovementDirection();

                    destination = 
                        hit.collider.transform.position + 
                        obstacleDirection * target.localScale.y / 2.0f +
                        obstacleDirection * offsetWhileNavigating;
                }
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                movementSpeed * Time.deltaTime
            );

            Debug.DrawLine(transform.position, destination, Color.red);

            RotateTowards(destination);
        }
    }
}
