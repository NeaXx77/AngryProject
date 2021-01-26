using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryProject
{
    public class EnnemyStun : MonoBehaviour
    {
        #region Vars
        public float timeToRecover;
        float timerToRecover;
        bool isRecovering = false;
        CircleCollider2D circleCollider;
        BoxCollider2D boxCollider;
        #endregion
        BatAI bataiComp = null;
        FireSkullController skullController = null;
        Animator animator = null;
        public GameObject windEffectorZibou = null;
        public GameObject particles = null;
        //autre
        private void Awake() {
            timerToRecover = timeToRecover;
            animator = GetComponent<Animator>();
            if(bataiComp = GetComponent<BatAI>()){
                circleCollider = GetComponent<CircleCollider2D>();
                boxCollider = GetComponent<BoxCollider2D>();
            }else{
                boxCollider = GetComponent<BoxCollider2D>();
            }
        }
        private void Update() {
            if(isRecovering){
                if(timerToRecover > 0){
                    timerToRecover -= Time.deltaTime;
                }else{
                    if(circleCollider != null)
                        circleCollider.enabled = true;
                    if(boxCollider != null)
                        boxCollider.enabled = true;
                    timerToRecover = timeToRecover;
                    isRecovering = false;
                    animator.SetBool("isStun",false);

                    if(windEffectorZibou){
                        windEffectorZibou.SetActive(true);
                    }
                }
            }
        }
        public void EnableMove(){
            bataiComp.enabled = true;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Player")){
                if(bataiComp != null && !isRecovering){ //Bat
                    bataiComp.enabled = false;
                    BatObject();
                    animator.SetBool("isStun",true);
                }else if(skullController)   //FireSkull
                {
                    
                }else if(!isRecovering) //Zibou
                {
                    ZibouObject();
                    animator.SetBool("isStun",true);
                    particles.SetActive(false);
                    windEffectorZibou.SetActive(false);
                }
            }
        }

        void BatObject(){
            circleCollider.enabled = false;
            boxCollider.enabled = false;
            isRecovering = true;
        }

        void ZibouObject(){
            boxCollider.enabled = false;
            isRecovering = true;
        }

        public void ActivateParticles(){
            particles.SetActive(true);
        }
    }
}