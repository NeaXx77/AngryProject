using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngryProject;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameObject playerInstance = null;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject victoryUI;
    public GameObject player;
    bool isGameOver = false;
    int collectable;
    public int actualWorld;
    public int actualLevel;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null){
            player = GameObject.Find("Cat");
        }
        if(instance != this){
            instance = this;
        }
        playerInstance = player;
    }

    public static Vector2 CalculatePlayerPos(Transform from){
        Vector2 playerPos = GameManager.playerInstance.transform.position;
        Vector2 move = playerPos - (Vector2)from.position;

        return move;
    }

    public void SetActiveMenu(int menu){
        switch (menu)
        {
            case Constants.PAUSEMENU:
                if(!pauseMenu.activeInHierarchy)
                    pauseMenu.SetActive(true);
                else
                    pauseMenu.SetActive(false);
                break;
            case Constants.GAMEOVERMENU:
                if(!gameOverMenu.activeInHierarchy)
                    gameOverMenu.SetActive(true);
                else
                    gameOverMenu.SetActive(false);
                break;
            default:
                break;
        }
    }
    public bool GetIsGameOver(){
        return isGameOver;
    }

    public void AddCollectable(){
        collectable++;
    }

    public void OnVictory(){
        TimeManager.instance.SetNormalTime();
        TimeManager.instance.gameObject.SetActive(false);
        victoryUI.SetActive(true);
        PlayerPrefs.SetInt("LevelVictory",1);
        PlayerPrefs.GetInt("World",actualWorld);
        PlayerPrefs.GetInt("Level",actualLevel);
        PlayerPrefs.GetInt("Stars",collectable);
    }
}
