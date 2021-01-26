using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RpgEssentials;

namespace AngryProject
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField]private ContainerBar healthBar;
        public int lifeCount;
        public float invincibilityTime;
        float invincibilityTimer;
        Animator catAnimator;
        public Animator globalAnimator;
        void Start()
        {
            if(healthBar == null){
                healthBar = GameObject.Find("LifeContainer").GetComponent<ContainerBar>();
            }
            lifeCount = (int)healthBar.GetCurrentAmount();
            CustomGameEvent.instance.onPlayerDeath += OnDeath;
            catAnimator = GetComponent<Animator>();
        }


        // Update is called once per frame
        void Update()
        {
            if(lifeCount == 0 && !GameManager.instance.gameOverMenu.activeInHierarchy){
                CustomGameEvent.instance.PlayerDeath();
            }
            if(invincibilityTimer > 0)
                invincibilityTimer -= Time.deltaTime;
        }
        public void OnDeath(){
            GameManager.instance.SetActiveMenu(Constants.GAMEOVERMENU);
            DragController.instance.SetIsAlive(false);
        }
        public void TakeDamage(){
            if(invincibilityTimer <= 0){
                healthBar.ChangeBarCurrentAmount(-1);
                lifeCount--;
                invincibilityTimer = invincibilityTime;
                catAnimator.SetTrigger("Hurt");
                catAnimator.SetBool("isHurt",true);
                globalAnimator.SetTrigger("CatHurt");
                print(globalAnimator.gameObject.name);
            }
        }
        public float GetInvincibleTimer(){
            return invincibilityTimer;
        }
    }
}