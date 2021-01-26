using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFunctions : MonoBehaviour
{
    public GameObject[] UI;

    public void DisableUI(){
        foreach (GameObject item in UI)
        {
            item.SetActive(false);
        }
    }
    public void EnableUI(){
        foreach (GameObject item in UI)
        {
            item.SetActive(true);
        }
    }
}
