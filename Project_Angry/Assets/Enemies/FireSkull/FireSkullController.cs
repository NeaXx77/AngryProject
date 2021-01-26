using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryProject
{
    public class FireSkullController : MonoBehaviour
    {//Not kinematic but 0 gravity scale
        //Idle - Player in Agro area? 
        //          Yes: transform in flames -> Dash(animation + add force in playerDir predicted) -> flames out -> turn -> check if still in Second agro area -> return at the beginning
        //          No: Go back on the "spawn area"
        //Between attacks: floating in random direction to the player then dash
        #region States
        bool isIdle = true;
        bool isAgro = false;
        bool isDashing = false;
        bool isReachable = true;
        bool isHit = false;
        bool isSlowMoSpeed = false;
        bool lookRight = true;
        #endregion
        
        public LayerMask GroundMask;

        #region Components
        public Rigidbody2D skullRb;
        public LayerMask playerMask;
        public Animator skullAnimator;
        #endregion

        #region Attributs

        [Header("Attributes")]
        public float idleRadius = 7f;
        private Vector2 idleOrigin;
        public float agroRadius = 5f;
        float stayAgroRadius;
        public float force = 1;
        public float decelerateRatio = 1.1f;
        public float decelerateOffsetSlowMotion = 0.0005f;
        public float timeBeforeMove = 1f;
        private float timerBeforeMovingSkull = 1f;
        #endregion
        
        private void Start() {
            timerBeforeMovingSkull = timeBeforeMove;
            skullRb = GetComponentInParent<Rigidbody2D>();
            idleOrigin = GetComponentInParent<Transform>().position;
            stayAgroRadius = agroRadius + 5;
        }

        private void Update() {
            if(!isAgro)
                isAgro = Physics2D.OverlapCircle(transform.position, agroRadius, playerMask);
            else
                isAgro = Physics2D.OverlapCircle(transform.position, stayAgroRadius, playerMask);

            //When speed magnitude is <= 0.... something, stop the move, stop flame/dash anim and return to agro state
            if(isAgro && !isDashing){
                //Start combust animation
                StopAllCoroutines();
                skullAnimator.SetTrigger("Spoted");
                SetSpriteDir(true);
            }else if(isIdle && !isDashing && !isHit){
                IdleMoving();
            }
            Decelerate();
            StopSkull();
            if(isDashing){
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }else{
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            SetRotationFromVelocity();
            skullRb.velocity = new Vector2(Mathf.Clamp(skullRb.velocity.x,-20,20), Mathf.Clamp(skullRb.velocity.y,-20,20));
        }

        public void SetSpriteDir(bool playerDir){
            if(!playerDir){
                Vector2 scale = new Vector2(skullRb.velocity.x>0? -1:1, 1);
                GetComponentInChildren<Transform>().localScale = scale;
            }else{
                Vector2 dirPlayer = GameManager.CalculatePlayerPos(transform).normalized;
                if(dirPlayer.x < 0){
                    GetComponentInChildren<Transform>().localScale = new Vector2(1,1);
                }else
                {
                    GetComponentInChildren<Transform>().localScale = new Vector2(-1,1);
                }
            }
        }

        //On animation which start to be in flame and stop
        public void SetIsReachable(bool state){
            this.isReachable = state;
        }

        public void SetIsIdle(bool state){
            this.isIdle = state;
        }
        //on animation dash
        public void DashThroughPlayer(){
            float slowMotionRate = 1/Time.timeScale;
            //set animation start dash
            isDashing = true;
            //add force
            Vector2 dirForce = GameManager.CalculatePlayerPos(transform).normalized;
            if(Time.timeScale < 1){
                dirForce *= force*slowMotionRate;
            }else{
                dirForce *= force;
            }

            skullRb.velocity = dirForce;
            Vector2 scale = new Vector2(skullRb.velocity.x>0? -1:1, 1);
            GetComponentInChildren<Transform>().localScale = scale;
        }
        public Animator GetAnimator(){
            return this.skullAnimator;
        }
        public void SetIsDashing(bool state){
            isDashing = state;
        }
        public void SetIsReachablee(bool state){
            isReachable = state;
        }

        public bool getIsReachable(){
            return this.isReachable;
        }

        public void SetDashingAnimator(){
            skullAnimator.SetTrigger("Dash");
        }
        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, agroRadius);
        }

        public void IdleMoving(){
            if(timerBeforeMovingSkull > 0){
                timerBeforeMovingSkull -= Time.deltaTime;
            }else{
                timerBeforeMovingSkull = timeBeforeMove;
                float newX,newY;
                newX = Random.Range(idleOrigin.x - idleRadius, idleOrigin.x + idleRadius) + 0.5f;
                newY = Random.Range(idleOrigin.y - idleRadius, idleOrigin.y + idleRadius) + 0.5f;
                Vector2 newPos = new Vector2(newX, newY);                
                StartCoroutine(MoveThrough(newPos));
            }
        }
        IEnumerator MoveThrough(Vector2 position){
            Vector2 vecPos = (Vector2)transform.position - position;
            Vector2 scale = new Vector2(position.x > transform.position.x ? -1:1, 1);
            
            GetComponentInChildren<Transform>().localScale = scale;

            while(vecPos.magnitude > 0.5f){
                transform.position = Vector2.MoveTowards(transform.position,position,0.19f);
                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if(CompareTag("Player")){
                Vector2 dirForce = GameManager.CalculatePlayerPos(transform).normalized*force/10;
                skullRb.velocity = new Vector2(-dirForce.x,-dirForce.y);
            }
        }

        private void Decelerate(){
            if(Time.timeScale == 1)
                skullRb.velocity = (skullRb.velocity * (decelerateRatio));
            else
                skullRb.velocity = (skullRb.velocity * (decelerateRatio + decelerateOffsetSlowMotion + ( ( (1-decelerateRatio) / (1/Time.timeScale) ))));

            if(isAgro && skullRb.velocity.magnitude < 2f){
                skullRb.velocity = Vector2.zero;
                isDashing = false;
                if(!isAgro){
                    isIdle = true;
                    MoveThrough(idleOrigin);
                }else{
                    skullAnimator.SetTrigger("Spoted");
                    skullAnimator.SetBool("Idle",!isAgro);
                    
                    SetSpriteDir(true);
                }
                skullAnimator.SetTrigger("Uncombust");
            }
        }

        public void StopSkull(){
            if(skullRb.velocity.magnitude < 1.8f){
                skullRb.velocity = Vector2.zero;
                isDashing = false;
                skullAnimator.SetTrigger("Uncombust");
            }
        }

        void SetRotationFromVelocity(){
            float rayDistance = 2;
            RaycastHit2D downRay = Physics2D.Raycast((Vector2)transform.position, Vector2.down , rayDistance, GroundMask);
            if(skullRb.velocity.magnitude > 0.02f){
                if(skullRb.velocity.x < 0){
                    transform.localScale = new Vector3(-1,-1,0);
                    lookRight = false;
                }else if(skullRb.velocity.x > 0)
                {
                    transform.localScale = new Vector3(-1,1,0);
                    lookRight = true;
                }
            }else
            {
                transform.localScale = new Vector3(1,transform.localEulerAngles.z == 0 ?1:-1,0);                
            }
            if(Mathf.Abs(skullRb.velocity.magnitude) > 0.05f){
                Vector2 dir = skullRb.velocity.normalized;
                float angle = Mathf.Atan2(dir.x,dir.y)*Mathf.Rad2Deg;
                //Si Velocité.y < seuil && ray hit touche un sol, rotation.z = 0 || 180

                if(skullRb.velocity.y < 1.5f && downRay.collider != null){
                    transform.eulerAngles = new Vector3(0,0, lookRight?0:180);
                }else if(skullRb.velocity.x != 0 || Mathf.Abs(skullRb.velocity.magnitude) > 5f) {
                    transform.eulerAngles = new Vector3(0,0,-angle+90);
                }
            }else{
                skullRb.velocity = Vector2.zero;
            }
        }
    }    
}
