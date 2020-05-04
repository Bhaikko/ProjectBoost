using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 1.0f;
    [SerializeField] float rotateSpeed = 1.0f;

    Rigidbody myRigidbody = null;
    AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        // Returns true when Key is held down
        if (Input.GetKey(KeyCode.Space)) {
            myRigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
