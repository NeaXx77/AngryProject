using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryProject
{
    [RequireComponent(typeof(PlayerHealthController))]
    [RequireComponent(typeof(Animator))]
    public class CollisionController : MonoBehaviour
    {
        //Layer
        int neutral = 9;
        
        //Other
        public GameObject particleGroundHit = null;
        PlayerHealthController healthController;
        DragController controller;
        CircleCollider2D catCollider;
        GameObject gObject;
        Animator catAnimator;
        public float speedTreshold = 1;
        [Range(0.5f,1)]
        public float bouncingRatio = 1;
        public float animateMagnitude = 2;
        public float timeToConsiderSlide = 1;
        private float timerToConsiderSlide = 1;
        public float hurtingKnockBack = 5;
        public int timeBeforeIsHurtFalse;
        public int starttimeBeforeisHurt = 3;
        float minOffsetCol = 0.1f;
        float airTime = 0;
        public Animator parentAnimator = null;
        private void Awake() {
            catAnimator = GetComponent<Animator>();
            healthController = GetComponent<PlayerHealthController>();
            controller = GetComponent<DragController>();
            catCollider = GetComponent<CircleCollider2D>();
            timeBeforeIsHurtFalse = starttimeBeforeisHurt;
        }
        private void Update() {
            Animate();
            if(controller.GetDownRay().collider == null && airTime < 0.7f){
                airTime += Time.deltaTime;
            }
        }
        private void FixedUpdate() {
            catAnimator.ResetTrigger("SideHit");
            catAnimator.ResetTrigger("VerticalHit");
        }
        private void OnTriggerEnter2D(Collider2D other) {
            float bouncingY;
            if(other.CompareTag("Hurting")){
                
                healthController.TakeDamage();
                TimeManager.instance.SetTimeScaleWithTimer(0,0.2f);
                
                //rebondir
                bouncingY = other.transform.localScale.y == 1 ? hurtingKnockBack : -hurtingKnockBack;
                controller.GetRigidbody2D().velocity = new Vector2(controller.GetRigidbody2D().velocity.x, bouncingY);
            
            }else if(other.CompareTag("Bouncing"))
            {
                FroggyAI froggy;
                if(other.gameObject.layer != neutral){
                    TimeManager.instance.SetTimeScaleWithTimer(0,0.2f);
                }else if(froggy = other.GetComponent<FroggyAI>()){
                    froggy.PlayerCollide();
                }
                controller.AddJumpCount();
                
                bouncingY = controller.GetRigidbody2D().velocity.y < -12 ? -(controller.GetRigidbody2D().velocity.y * bouncingRatio) : (other.GetComponent<ObjectData>().data.bouncingForce);
                controller.GetRigidbody2D().velocity = new Vector2(controller.GetRigidbody2D().velocity.x, bouncingY);
            }
            ISoundReaction s = other.GetComponent<ISoundReaction>();
            if(s != null){
                s.MakeSound(gameObject);
            }
            if(other.gameObject.CompareTag("Finish")){
                controller.GetRigidbody2D().velocity = new Vector2(0,0);
                controller.canMove = false;
            }
        }
        
        //Mettre un box collider pour la collision de coté
        private void OnCollisionEnter2D(Collision2D other) {
            Vector2 collisionPosition = other.GetContact(0).point;
            if(controller != null && controller.GetRigidbody2D() != null){
                if((collisionPosition.y > (transform.position.y) + minOffsetCol) || (collisionPosition.y < (transform.position.y) - minOffsetCol)){
                    //Haut ou bas
                    float speedMagnitude = Mathf.Abs(controller.GetRigidbody2D().velocity.y); 
                    
                    if(speedMagnitude > speedTreshold && (airTime > 0.7f || (speedMagnitude > 11 && airTime > 0.4f))){ //Faire que si il est en isFlying depuis 1 s
                        catAnimator.SetTrigger("VerticalHit");
                        if(controller.GetRigidbody2D().velocity.y > 0){
                            gObject = Instantiate(particleGroundHit, collisionPosition, Quaternion.identity);
                            gObject.transform.localEulerAngles = new Vector2(-90, 0);
                        }
                    }
                }else if((controller.GetLookingRight() && collisionPosition.x > (transform.position.x) + minOffsetCol) 
                            || (!controller.GetLookingRight() && collisionPosition.x < (transform.position.x) - minOffsetCol) ){
                    //Coté
                    float xVel = controller.GetRigidbody2D().velocity.x;
                    if(Mathf.Abs(xVel) > speedTreshold && airTime > 0.7f){
                        
                        catAnimator.SetTrigger("SideHit");
                    }
                }

                FireSkullController skullController;
                if((skullController = other.gameObject.GetComponent<FireSkullController>()) != null 
                    && skullController.getIsReachable() 
                    && other.gameObject.transform.position.y < transform.position.y){

                    float bouncingY = controller.GetRigidbody2D().velocity.y < -12 ? -(controller.GetRigidbody2D().velocity.y * bouncingRatio) : (other.gameObject.GetComponent<ObjectData>().data.bouncingForce);
                    
                    controller.GetRigidbody2D().velocity = new Vector2(controller.GetRigidbody2D().velocity.x, bouncingY);
                }else if((skullController = other.gameObject.GetComponent<FireSkullController>()) != null 
                    && !skullController.getIsReachable() ){
                        healthController.TakeDamage();
                    }
            }
            
            if(other.gameObject.CompareTag("Ground") && !controller.canMove){
                print("victory");
                GetComponent<Animator>().SetBool("Victory",true);
                transform.position = new Vector2(other.GetContact(0).point.x + 2, other.GetContact(0).point.y + 2);
                controller.GetRigidbody2D().velocity = Vector2.zero;
                controller.GetRigidbody2D().isKinematic = true;
                transform.rotation = Quaternion.Euler(0,0,0);
                controller.enabled = false;
                GameManager.instance.OnVictory();
                Time.timeScale = 1;
            }
            airTime = 0;
        }

        void Animate(){
            RaycastHit2D controllerDownRay = controller.GetDownRay();
            if(Mathf.Abs(controller.GetRigidbody2D().velocity.magnitude) > animateMagnitude){
                catAnimator.SetBool("isFlying",true);
            }else
            {
                catAnimator.SetBool("isFlying",false);
            }

            if(Mathf.Abs(controller.GetRigidbody2D().velocity.x) < 2f){
                if(timerToConsiderSlide > 0){
                    timerToConsiderSlide -= Time.deltaTime;
                }else
                {
                    catAnimator.SetBool("isSliding",true);
                }
            }
            if(controllerDownRay.collider == null){
                catAnimator.SetBool("isSliding",false);
                timerToConsiderSlide = timeToConsiderSlide;
            }
        }

        public void StopCat(){
            if(!controller.getIsDraging()){
                controller.GetRigidbody2D().velocity *= 0.5f;
            }
        }

        public void SetAnimatorIsHurtTrue(){
            timeBeforeIsHurtFalse -= 1;
            if(timeBeforeIsHurtFalse == 0){
                catAnimator.SetBool("isHurt",false);
                timeBeforeIsHurtFalse = starttimeBeforeisHurt;
            }
        }
    }
}