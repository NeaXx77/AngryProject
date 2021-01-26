using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    Vector2 playerSpawn;
    private void Start() {
        playerSpawn = GameObject.Find("Cat").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        other.transform.position = playerSpawn;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
