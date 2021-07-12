using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.Environment
{
    public class EndGate : MonoBehaviour
    {
        [SerializeField] bool bShouldClose = false;

        [SerializeField] float rotationSpeed = 10.0f;    
        [SerializeField] float angleToRotate = 60.0f;


        float angleRotated = 0.0f;

        void Start()
        {
    
        }

        void Update()
        {
            if (bShouldClose) {
                Rotate();
            }
        }

        private void Rotate()
        {
            angleRotated += rotationSpeed * Time.deltaTime;
            transform.Rotate(
                -Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World
            );

            if (angleRotated >= angleToRotate) {
                bShouldClose = false;
                angleRotated = 0.0f;
            }
        }

        public void CloseDoor() {
            bShouldClose = true;
        }
    }    
}
