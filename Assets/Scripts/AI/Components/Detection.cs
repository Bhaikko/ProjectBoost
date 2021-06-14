using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBoost.AI {
    public class Detection : MonoBehaviour
    {
        [SerializeField] AudioClip detectionSound = null;

        Animation animationToPlay = null;
        AudioSource audioSource = null;


        private void Start() {
            audioSource = GetComponent<AudioSource>();

            animationToPlay = GetComponent<Animation>();
            animationToPlay.playAutomatically = false;
        }

        public void PlayAnimation() 
        {
            animationToPlay.Play();
            audioSource.PlayOneShot(detectionSound);
        }
     
    }
}
