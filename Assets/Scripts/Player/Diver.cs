using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Core;
using ProjectBoost.Environment;
using ProjectBoost.SceneManagement;
using ProjectBoost.UI;
using UnityEngine;

namespace ProjectBoost.Player {

    public class Diver : MonoBehaviour
    {
        [SerializeField] float thrustSpeed = 1.0f;
        [SerializeField] float rotateSpeed = 1.0f;

        [SerializeField] AudioClip thrustingSound = null;
        // [SerializeField] AudioClip deathSound = null;

        [SerializeField] ParticleSystem thrustParticleSystem = null;

        Rigidbody myRigidbody = null;
        AudioSource audioSource = null;
        Animator diverAnimator = null;

        private GameMode gameMode;
        private Vector3 lastHidingPosition = Vector3.negativeInfinity;
        private Vector3 deathAttachPosition;
        private GameObject killer;

        private bool isDead = false;
        private bool isHiding = false;
        private bool isLevelFinished = false;

        private Vector3 levelEndPosition;

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
            thrustParticleSystem.transform.rotation = Quaternion.Euler(Vector3.up);
            if (isLevelFinished) {
                TranslateToLevelEnd();
                StartCoroutine(LoadNextScene());
                return;
            }

            if (!isDead) { 
                Thrust();
                Rotate();
            } else {
                if (killer) {
                    transform.position = killer.transform.position;
                    transform.rotation = killer.transform.rotation;
                } 
            }
        }

        public void FinishLevel(Vector3 levelEndPosition) {
            isLevelFinished = true;
            this.levelEndPosition = levelEndPosition;
        }

        private void TranslateToLevelEnd() {
            transform.position = Vector3.MoveTowards(transform.position, levelEndPosition, Time.deltaTime * thrustSpeed);
        }

        private void Rotate()
        {
            //myRigidbody.freezeRotation = true;  // Take manual Control of rotation
            myRigidbody.angularVelocity = Vector3.zero;

            diverAnimator.SetFloat("Horizontal", Input.GetAxis("Rotate"));
            
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
                // if (!thrustParticleSystem.isPlaying) { 
                //     thrustParticleSystem.Play();
                // }
            } else {
                audioSource.Stop();
                // thrustParticleSystem.Stop();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isDead || isLevelFinished) { 
                return; 
            }

            StartCoroutine(HandleDeath());
            
        }

        IEnumerator LoadNextScene()
        {
            isDead = true;
            yield return new WaitForSeconds(1.5f);
            FindObjectOfType<Fader>().FadeOut();

            gameMode.LoadNextScene();
        }

        IEnumerator HandleDeath()
        {
            isDead = true;
            audioSource.Stop();
            // audioSource.PlayOneShot(deathSound);
            // thrustParticleSystem.Stop();
            yield return new WaitForSeconds(2.5f);
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

        public void SetKiller(ref GameObject killer) {
            this.killer = killer;

            // GetComponent<CapsuleCollider>().enabled = false;
        }

    } 
}
