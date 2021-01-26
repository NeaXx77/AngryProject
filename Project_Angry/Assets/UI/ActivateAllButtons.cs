using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAllButtons : MonoBehaviour
{
    Button[] buttons;

    private void Start() {
        buttons = GetComponentsInChildren<Button>();
    }
    public void EnableButtons(){
        foreach (Button obj in buttons)
        {
            obj.enabled = true;
        }
    }
}
