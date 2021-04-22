﻿using System.Collections;
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
        Animator diverAnimator = null;

        private GameMode gameMode;
        private Vector3 lastHidingPosition = Vector3.negativeInfinity;

        private bool isDead = false;
        private bool isHiding = false;

        // Start is called before the first frame update
        void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            diverAnimator = GetComponentInChildren<Animator>();

            gameMode = FindObjectOfType<GameMode>();

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

            diverAnimator.SetFloat("Horizontal", Input.GetAxis("Rotate"));
            // transform.Rotate(-1.0f * Mathf.Ceil(Input.GetAxis("Rotate")) * Vector3.forward, rotateSpeed * Time.deltaTime);
            
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
            if (Input.GetAxis("Thrust") > 0.0f) {
                
                diverAnimator.SetFloat("Vertical", Input.GetAxis("Thrust"));
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
            // audioSource.PlayOneShot(deathSound);
            thrustParticleSystem.Stop();
            deathParticleSystem.Play();
            isDead = true;
            yield return new WaitForSeconds(1.5f);
            // SceneManager.LoadScene(0);

            gameMode.HandleDeath();
        }

        public void SetHidingStatus(bool status, float hidingOffset = 0.0f) {
            if (status) {
                this.lastHidingPosition = transform.position + new Vector3(0.0f, hidingOffset, 0.0f);

            } else {
                this.lastHidingPosition = Vector3.negativeInfinity;
            }

            this.isHiding = status;
        }

        public bool IsHiding() { return isHiding; }
        public Vector3 GetLastHidingPosition() { return lastHidingPosition; }

    } 
}
