using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifManager : MonoBehaviour
{
    public float[] timerForNextAction;
    public Vector2[] dirJump;
    public GameObject objectToJump;
    private Rigidbody2D objectRb = null;
    float actualTimer;
    int index = 0;
    private void Start() {
        actualTimer = timerForNextAction[index];
        objectRb = objectToJump.GetComponent<Rigidbody2D>();
    }
    private void Update() {
        if(index < timerForNextAction.Length){
            if(actualTimer > 0){
                actualTimer -= Time.deltaTime;
            }else{
                objectRb.velocity = new Vector2(dirJump[index].x, dirJump[index].y);
                index++;
                actualTimer = timerForNextAction[index];
            }
        }
    }
}
