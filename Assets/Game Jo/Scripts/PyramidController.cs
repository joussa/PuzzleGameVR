using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidController : MonoBehaviour {

    //puzzle access
    public PuzzleController puzzle;

    // particle system
    public ParticleSystem particles;

    //duration
    public float duration = 5;

    // movement flag
    bool isMoving = false;

    // accumulated time
    float accTime = 0;

    // speed
    float speed;

    // final position
    float endY = 0;

    void Start()
    {
        // speed = d / t
        float distance = endY - transform.position.y;
        speed = distance / duration;
    }

    void OnEnable()
    {
        puzzle.OnCompleted += Raise;
    }

    void OnDisable()
    {
        puzzle.OnCompleted -= Raise;
    }

    void Raise()
    {
        // start moving
        isMoving = true;

        // play the particle effect
        particles.Play();
    }

    void Update()
    {
        // only move if the flag is true
        if (!isMoving) return;

        // increase accumulated time
        accTime += Time.deltaTime;

        // movement on Y
        float movement = speed * Time.deltaTime;

        // move the pyramid
        transform.Translate(Vector3.up * movement);

        // check that it's time to stop
        if (accTime >= duration)
        {
            // stop moving
            isMoving = false;
        }
    }


}
