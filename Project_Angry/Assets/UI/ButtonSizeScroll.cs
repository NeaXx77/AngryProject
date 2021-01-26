using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonSizeScroll : MonoBehaviour
{
    Button[] buttons;
    public float centerPos;
    [SerializeField][Range(0.1f,1)]float minScale = 0.75f; 
    ScrollRect scrollRect = null;
    public GameObject[] levels;
    float scrollbar;
    bool mouseUp = false;
    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        scrollRect = GetComponent<ScrollRect>();
        SetInteractableAccordingToScale();
    }
    private void Update() {
        ResizeButton();
        centerPos = GetComponentInParent<Canvas>().transform.position.x;
        
        if(Input.GetMouseButtonUp(0)){
            mouseUp = true;
        }else if (Input.GetMouseButtonDown(0)){
            mouseUp = false;
        }
        if(mouseUp){
            for (int i = 0; i < buttons.Length; i++)
            {
                float nbScrollValue = i*(1f/(buttons.Length-1f));
                if(buttons[i].transform.localScale.x > minScale){
                    GetComponentInChildren<Scrollbar>().value = Mathf.Lerp(GetComponentInChildren<Scrollbar>().value,
                                                                            nbScrollValue, 
                                                                            0.1f);
                }
                SetInteractableAccordingToScale();
            }
        }
    }
    public void ResizeButton(){
        foreach (Button but in buttons)
        {
            float posX = but.transform.position.x;
            if(posX > centerPos){
                posX = centerPos - (posX-centerPos);
            }
            Vector2 scale = new Vector2(posX/centerPos, posX/centerPos);
            scale = new Vector2(Mathf.Clamp(scale.x, minScale, 1), Mathf.Clamp(scale.y, minScale, 1));
            but.transform.localScale = scale; 
        }

        
    }
    public void SetScrollRectInteractable(bool state){
        scrollRect.horizontal = state;
        //Animate UI to make appear levels
    }
    public void SetActiveLevels(int i){
        levels[i].SetActive(true);
    }
    void SetInteractableAccordingToScale(){
        int worldToEnable = LevelUnlockManager._levelManager.GetLevelUnlocked()/10 + 1;
        for (int i = 0; i < buttons.Length; i++)
            if(buttons[i].transform.localScale.x >= 0.98f && i < worldToEnable){
                buttons[i].interactable = true;
            }else{
                buttons[i].interactable = false;
            }
    }
}
