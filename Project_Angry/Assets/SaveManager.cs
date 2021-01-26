using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager _saveManager;

    [SerializeField]
    private GameObject[] worldLevels = null;
    public LevelStars[][] levelsStarsWorld = new LevelStars[4][];

    private void Start() {
        _saveManager = this;
        GetAllLevelStars();
        if(PlayerPrefs.GetInt("AlreadySaved",0) == 1){
            LoadData(levelsStarsWorld);
        }else
        {
            StartCoroutine(FirstSaveDelay());
            PlayerPrefs.SetInt("AlreadySaved",1);
        }

        if(PlayerPrefs.GetInt("LevelVictory",0) == 1){
            PlayerPrefs.SetInt("LevelVictory",0);
            levelsStarsWorld[PlayerPrefs.GetInt("World",0)][PlayerPrefs.GetInt("Level",0)].SetStars(PlayerPrefs.GetInt("Stars",0));
        }
    }
    public void SaveData(){
        SaveSystem.Save();
    }
    //Save avec playerpref à chaques fin de level
    public void LoadData(LevelStars[][] stars){
        SaveData data = SaveSystem.LoadData();

        if(data != null){
            LevelUnlockManager._levelManager.levelsUnlock = data.levelUnlocked;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    levelsStarsWorld[i][j].SetStars(data.starsPerLevel[i][j]);
                }
            }
        }
        PlayerPrefs.SetInt("Collectable", data.nbCollectable);
    }

    private void GetAllLevelStars(){
        for (int i = 0; i < 4; i++)
        {
            levelsStarsWorld[i] = worldLevels[i].GetComponentsInChildren<LevelStars>();
            worldLevels[i].SetActive(false);
        }
    }

    IEnumerator FirstSaveDelay(){
        yield return new WaitForSeconds(1f);
        SaveData();
    }
}
