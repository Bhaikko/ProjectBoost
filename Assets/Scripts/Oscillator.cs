using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for moving Gameobject
[DisallowMultipleComponent] // Allow only one of this component on a gameobject
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movement = new Vector3();
    [SerializeField] float period = 2f;

    [Range(0, 1)]
    [SerializeField] float movementFactor = 0.0f;

    Vector3 startingPos = new Vector3();

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) {
            return;
        }

        float cycles = 1.0f / period;
        const float tau = Mathf.PI * 2;
        float sinValue = Mathf.Sin(cycles * tau * Time.time);       // x(t) = Asin(wt) = Asin(2 * pie * v * t);

        movementFactor = sinValue * 0.5f + 0.5f;        
        Vector3 offset = movement * movementFactor;
        transform.position = startingPos + offset;
    }
}
