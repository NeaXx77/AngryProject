using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AngryProject;
using UnityEngine.SceneManagement;
public class ButtonsScript : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIsInteractable(bool state){
        button.interactable = state;
    }

    public void SetActivePauseMenu(){
        GameManager.instance.SetActiveMenu(Constants.PAUSEMENU);
    }

    public void DeactivateMenu(Button button){
        GameManager.instance.SetActiveMenu(Constants.PAUSEMENU);
        if(button != null)
            button.interactable = true;
    }

    public void LoadScene(int id){
        if(id != -1)
            SceneManager.LoadScene(id);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
