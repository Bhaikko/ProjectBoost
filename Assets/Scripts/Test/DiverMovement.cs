﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverMovement : MonoBehaviour
{
    Animator animator = null;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        
    }
}
