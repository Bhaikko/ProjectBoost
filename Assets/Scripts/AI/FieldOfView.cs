using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.AI {
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] float viewRadius;

        [SerializeField] [Range(0, 360)] float viewAngle;   
        [SerializeField] LayerMask playerMask;

        public Vector3 DirectionFromAngle(float angle, bool isGlobalAngle) {
            if (!isGlobalAngle) {
                angle += transform.eulerAngles.y;
            }            

            return new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0.0f
            );
        }

        public float GetViewRadius() { return viewRadius; }
        public float GetViewAngle() { return viewAngle; }

        void Start() {
            StartCoroutine(VisibilityTest());
        }

        IEnumerator VisibilityTest() {
            while (true) {
                CheckVisibilityToPlayer();
                yield return new WaitForSeconds(1.0f);
            }
        }

        bool CheckIsDiver(Transform transform) {
            return transform.GetComponent<Diver>() || transform.GetComponentInParent<Diver>();
        }

        void CheckVisibilityToPlayer() {
            Collider[] objectsInView = Physics.OverlapSphere(transform.position, viewRadius, playerMask);


            for (int i = 0; i < objectsInView.Length; i++) {
                Transform target = objectsInView[i].transform;

                if (CheckIsDiver(target)) {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2) {
                        Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget)) {
                            if (CheckIsDiver(hit.transform)) {
                                Debug.Log("Is Player");
                            }
                        }
                    }
                }
            }
        }
        
    }
}
