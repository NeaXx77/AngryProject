using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowPlayer : MonoBehaviour
{
    [SerializeField]private GameObject player = null;
    [SerializeField]private Vector3 offset = new Vector3(0,0,-10);
    public bool isFollowing = true;
    private void Awake() {
        if (player == null && isFollowing)
        {
            player = GameObject.Find("Cat");
        }
    }
    void LateUpdate()
    {
        if(player){
            transform.position = player.transform.position + offset;
        }
    }
}
