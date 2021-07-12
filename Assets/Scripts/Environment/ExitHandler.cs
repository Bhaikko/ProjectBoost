using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.Environment
{
    public class ExitHandler : MonoBehaviour
    {
        EndGate endGate;


        // Start is called before the first frame update
        void Start()
        {
            endGate = GetComponentInChildren<EndGate>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider collision) {
            Diver diver = collision.gameObject.GetComponent<Diver>();

            if (diver) {
                diver.FinishLevel(transform.position);
                endGate.CloseDoor();
            }
        }
    }
}
