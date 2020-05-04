using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 1.0f;
    [SerializeField] float rotateSpeed = 1.0f;

    Rigidbody myRigidbody = null;
    AudioSource audioSource = null;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) { 
            Thrust();
            Rotate();
        }
    }

    private void Rotate()
    {
        myRigidbody.freezeRotation = true;  // Take manual Control of rotation

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        myRigidbody.freezeRotation = false; // Resume physics control of rotation
    }

    private void Thrust()
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Safe");
                break;
            
            case "Finish":
                StartCoroutine(LoadNextScene());
                break;

            default:
                StartCoroutine(HandleDeath());
                break;
        }
    }

    IEnumerator LoadNextScene()
    {
        isDead = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScene = SceneManager.sceneCountInBuildSettings;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene((currentSceneIndex + 1) % totalScene);
    }

    IEnumerator HandleDeath()
    {
        isDead = true;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }
}
