using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Core;
using ProjectBoost.Environment;
using ProjectBoost.SceneManagement;
using UnityEngine;

namespace ProjectBoost.Player {

    public class Diver : MonoBehaviour
    {
        [SerializeField] float thrustSpeed = 1.0f;
        [SerializeField] float rotateSpeed = 1.0f;

        [SerializeField] AudioClip thrustingSound = null;
        [SerializeField] AudioClip deathSound = null;
        [SerializeField] AudioClip newLevelSound = null;

        [SerializeField] ParticleSystem thrustParticleSystem = null;
        [SerializeField] ParticleSystem deathParticleSystem = null;
        [SerializeField] ParticleSystem newLevelParticleSystem = null;

        Rigidbody myRigidbody = null;
        AudioSource audioSource = null;

        private GameMode gameMode;

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
            //myRigidbody.freezeRotation = true;  // Take manual Control of rotation
            myRigidbody.angularVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.A)) {
                transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            } else if (Input.GetKey(KeyCode.D)) {
                transform.Rotate(-Vector3.forward, rotateSpeed * Time.deltaTime);
            }
            
            //myRigidbody.freezeRotation = false; // Resume physics control of rotation
        }

        private void Thrust()
        {
            // Returns true when Key is held down
            if (Input.GetKey(KeyCode.Space)) {
                myRigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
                if (!audioSource.isPlaying) {
                    audioSource.PlayOneShot(thrustingSound);
                }
                if (!thrustParticleSystem.isPlaying) { 
                    thrustParticleSystem.Play();
                }
            } else {
                audioSource.Stop();
                thrustParticleSystem.Stop();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isDead) { return; }

            LandingPad landingPad = collision.gameObject.GetComponent<LandingPad>();
            if (landingPad) {
                switch (landingPad.GetPadType())
                {
                    case LandingPad.PadType.FRIENDLY:
                        // Might Consider Refuel Functionality
                        break;
                    
                    case LandingPad.PadType.LANDING:
                        StartCoroutine(LoadNextScene());
                        break;

                    default:
                        break;
                }
            } else {
                StartCoroutine(HandleDeath());
            }
        }

        IEnumerator LoadNextScene()
        {
            isDead = true;
            
            audioSource.PlayOneShot(newLevelSound);
            newLevelParticleSystem.Play();

            yield return new WaitForSeconds(1.5f);

            gameMode.LoadNextScene();
        }

        IEnumerator HandleDeath()
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSound);
            thrustParticleSystem.Stop();
            deathParticleSystem.Play();
            isDead = true;
            yield return new WaitForSeconds(1.5f);
            // SceneManager.LoadScene(0);

            gameMode.HandleDeath();
        }

        public void SetGamemodeRef(GameMode gameMode) {
            this.gameMode = gameMode;
        }
    } 
}
