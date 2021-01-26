using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : ISoundReaction
{
    [Header("Movement")]
    [SerializeField]private bool Moving = false;
    [SerializeField]private bool horizontalMove = true;
    public float speed;
    [Header("Do not modify")]
    public Vector2 startPosition = Vector2.zero;
    public Vector2 moveDir;
    private Animator batAnimator;
    public AudioSource boingSound;
    public AudioClip clip;
    private void Start() {
        SetSounds(boingSound);
        SetClips(clip);
        SetClips(boingSound.clip);
        startPosition = transform.position;
        if(horizontalMove){
        }
        batAnimator = GetComponent<Animator>();
    }
    private void Update() {
        float distance = Mathf.Min(Vector2.Distance((Vector2)transform.position , (startPosition + moveDir)), Vector2.Distance((Vector2)transform.position , (startPosition - moveDir)));
        distance /= 1.5F;
        distance = Mathf.Clamp(distance,0.15f,0.5f);
        transform.position = Vector2.MoveTowards(transform.position, startPosition + moveDir, speed*Time.deltaTime * distance);
        CheckEndOfTrajectory();
    }
    void CheckEndOfTrajectory(){
        if((Vector2)transform.position == startPosition + moveDir){
            moveDir *= -1;
        }
    }

    public Animator GetAnimator(){
        return this.batAnimator;
    }
}
