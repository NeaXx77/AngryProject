using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AngryProject;
public class CustomGameEvent : MonoBehaviour
{
    public static CustomGameEvent instance;

    private void Awake() {
        instance = this;
    }

    public event Action onPlayerDeath;
    public void PlayerDeath(){
        
        if(onPlayerDeath != null){
            onPlayerDeath();
        }
    }
}
