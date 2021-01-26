using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] AudioClip collectableSound = null;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            GameManager.instance.AddCollectable();
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        //Collectable sound
        AudioSource.PlayClipAtPoint(collectableSound, GameManager.instance.player.transform.position);
    }
}
