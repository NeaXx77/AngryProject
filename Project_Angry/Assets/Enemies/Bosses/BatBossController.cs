using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngryProject;
using RpgEssentials;
public class BatBossController : MonoBehaviour
{
    public float normalSpeed = 10;
    public float speedHitRatio;
    private float speed;
    public float hitTime = 1;
    public float hitTimerinvincible = 1;
    public int health;
    public float knockbackForce = 10;
    public Animator bossAnim;
    enum States
    {
        state1 = 1,
        state2 = 2,
        state3 = 3
    }
    States bossState;
    // Start is called before the first frame update
    void Start(){
        bossAnim = GetComponent<Animator>();
        speed = normalSpeed;
    }

    // Update is called once per frame
    // Animate the boss in the current matching state
    void Update(){
        if(hitTimerinvincible > 0){
            hitTimerinvincible -= Time.deltaTime;
        }else
        {
            speed = normalSpeed;
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && hitTimerinvincible <= 0){
            health--;
            other.GetComponent<DragController>().GetRigidbody2D().AddForce(Vector2.left * knockbackForce);
            hitTimerinvincible = hitTime;
            speed = normalSpeed*speedHitRatio;
        }
    }
}
