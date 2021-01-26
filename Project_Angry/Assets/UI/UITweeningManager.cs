using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UITweeningManager : MonoBehaviour
{
    public float delay;
    public float timeTo;
    int i = 0;
    public LeanTweenType inType;
    public LeanTweenType outType;
    public int type = 0;//0 : world 1: levels
    public float yScaleWorld = 0.7f;
    public UnityEvent onCompleteCallbackIn;
    public UnityEvent onCompleteCallbackOut;
    private Button[] buttons;
    RectTransform canvasHeight;
    //When the gameobject is enable
    private void OnEnable() {
        buttons = GetComponentsInChildren<Button>();
        EnableUI();
        canvasHeight = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void EnableUI(){
        float scrollValue = GetComponentInParent<Canvas>().GetComponentInChildren<Scrollbar>().value;
        //for each child UI set the tween
        foreach (Button obj in buttons)
        {
            if(type == 0)
                obj.enabled = false;
            obj.transform.localScale = Vector3.zero;
            if(type == 1 || scrollValue == (1f/buttons.Length)*i)
                LeanTween.scale(obj.gameObject, new Vector3(1,1,1), timeTo).setEase(inType).setDelay(delay*i).setOnComplete(OnCompleteIn);
            else
                LeanTween.scale(obj.gameObject, new Vector3(0.7f,0.7f,0.7f), timeTo).setEase(inType).setDelay(delay*i).setOnComplete(OnCompleteIn);
            i++;
        }
        i = 0;  
    }

    public void DisableUIWorlds() {
        LeanTween.scale(gameObject, new Vector3(1,yScaleWorld,1), timeTo).setEase(outType).setDelay(delay*i).setOnComplete(OnCompleteOut);
        print(canvasHeight.rect);
        LeanTween.move(GetComponentInParent<ScrollRect>().gameObject, new Vector3(GetComponentInParent<ScrollRect>().transform.position.x, canvasHeight.rect.height*0.5f,0), timeTo).setEase(outType).setDelay(delay*i).setOnComplete(OnCompleteOut);
        LeanTween.alphaCanvas(GetComponentInParent<ScrollRect>().GetComponent<CanvasGroup>(), 0.7f, timeTo).setEase(outType).setDelay(delay*i).setOnComplete(OnCompleteOut);
    }
    public void OnCompleteIn(){
        if(onCompleteCallbackIn != null){
            onCompleteCallbackIn.Invoke();
        }
    }

    public void OnCompleteOut(){
        if(onCompleteCallbackOut != null){
            onCompleteCallbackOut.Invoke();
        }
    }
    public void SetEnableButtonsWorld(){
        foreach (Button obj in buttons)
        {
            obj.enabled = true;
        }
    }
}
