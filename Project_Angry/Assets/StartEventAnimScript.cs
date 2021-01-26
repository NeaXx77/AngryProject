using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngryProject;
public class StartEventAnimScript : MonoBehaviour
{
    GameObject cat;
    private void Start() {
        cat = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    public void StartSetActive(){
        Camera.main.GetComponent<FollowPlayer>().enabled = true;
        DragController drag = DragController.instance;
        cat.GetComponent<DragController>().enabled = true;
        cat.GetComponent<Animator>().enabled = true;
    }

    public void SetKinematicCat(){
        cat.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
