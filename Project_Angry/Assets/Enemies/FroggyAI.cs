using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroggyAI : MonoBehaviour
{
    bool groundCheck;
    public LayerMask groundMask = new LayerMask();
    BoxCollider2D frogCollider = new BoxCollider2D();
    Rigidbody2D froggyRigid;
    bool isJumping = true;
    public float timeBetweenJump = 1;
    public Vector2 velocityForce = Vector2.zero;
    float timerBeforeJump = 1;
    public int jumpNumberBeforeRotate = 3;
    private int jumpLeft;
    private Animator froggyAnim;
    bool isRayExit = false;

    public Animator froggySquishAnimator = null;
    [Header("Collision with froggy")]
    public float squishSpeed = 1;
    public float squishMin = 0.6f;
    Transform frogTransform;
    public ParticleSystem groundParticle;
    void Start()
    {
        froggyAnim = GetComponent<Animator>();
        jumpLeft = jumpNumberBeforeRotate;
        timerBeforeJump = timeBetweenJump;
        froggyRigid = GetComponent<Rigidbody2D>();
        frogCollider = GetComponent<BoxCollider2D>();
        frogTransform = GetComponent<Transform>();
        frogTransform.localScale = new Vector2(-frogTransform.localScale.x,frogTransform.localScale.y);
    }

    void Update()
    {
        RaycastHit2D hit2D;
        groundCheck = hit2D = Physics2D.Raycast(transform.position, Vector2.down, (frogCollider.size.y), groundMask);
        if(!groundCheck && !isRayExit){
            isRayExit = true;
        }
        //set kinematic and remove velo from Vector.zero
        if(froggyRigid.velocity.y < 0 && groundCheck && isJumping){
            froggyRigid.isKinematic = true;
            froggyRigid.velocity = Vector2.zero;
            isJumping = false;
            timerBeforeJump = timeBetweenJump;

            Vector2 pos = new Vector2(transform.position.x, hit2D.point.y + (frogCollider.size.y) );
            transform.position = pos;
        }
        if(groundCheck && isRayExit){
            froggyAnim.SetBool("isGrounded", groundCheck);
            isRayExit = false;
        }
        //Addforce
        if(timerBeforeJump <= 0 && !isJumping){
            froggyAnim.SetTrigger("Jump");
        }else
        {
            timerBeforeJump -= Time.deltaTime;
        }
        if(jumpLeft == 0){
            jumpLeft = jumpNumberBeforeRotate;
            velocityForce.x *= -1;
        }
    }
    public void Jump(){
        jumpLeft--;
        froggyRigid.isKinematic = false;
        isJumping = true;
        froggyRigid.velocity = (velocityForce);
        froggyAnim.ResetTrigger("Jump");
        froggyAnim.SetBool("isGrounded", false);
    }

    //On animation
    public void TurnAround(){
        if(jumpLeft == jumpNumberBeforeRotate){
            transform.localScale = new Vector2(-transform.localScale.x,1);
        }
    }

    public void PlayerCollide(){
        froggySquishAnimator.SetTrigger("Squish");
    }

    //On animation
    public void PlayParticle(){
        groundParticle.Play();
    }
}
