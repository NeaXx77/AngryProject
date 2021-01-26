using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class EnduranceOpacity : MonoBehaviour
{
    public float disapearringTime;
    Image endBar = null;
    Color color = Color.blue;
    void Start()
    {
        endBar = GetComponent<Image>();
        color.a = 0;
        endBar.color = color;
    }

    void Update()
    {
        if(endBar.fillAmount >= 1 && endBar.color.a > 0){
            color.a -= Time.deltaTime / disapearringTime;
        }else if(endBar.fillAmount < 1){
            color.a = 1;
        }
        color.a = Mathf.Clamp01(color.a);
        endBar.color = color;
    }
}
