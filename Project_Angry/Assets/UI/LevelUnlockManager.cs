using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager _levelManager = null;

    [SerializeField] 
    private Button[] worldsButtons = null;

    [SerializeField] 
    private GameObject[] levelsParentGameObject = null;
    private int levelUnlocked = 2;
    public int levelsUnlock {
        get{
            return levelUnlocked;
        }
        set{
            levelUnlocked = value;
        }
    }
    private void Start() {
        if(_levelManager == null){
            _levelManager = this;
        }
        //Récupérer de la save le nombre de level unlock
        PlayerPrefs.SetInt("levelUnlocked",2);
        levelUnlocked = PlayerPrefs.GetInt("levelUnlocked",2);
        UnlockWorlds();
        if(PlayerPrefs.GetInt("NewLevelUnlocked",0) == 1){
            PlayerPrefs.SetInt("NewLevelUnlocked",0);
            levelUnlocked++;
            PlayerPrefs.SetInt("levelUnlocked",levelUnlocked);
        }
    }

    public void UnlockWorlds(){
        int worldToUnlock = levelUnlocked/10 +1;
        for (int i = 0; i < worldsButtons.Length; i++)
        {
            if(i < worldToUnlock)
                worldsButtons[i].interactable = true;
            else{
                worldsButtons[i].interactable = false;
            }
        }
    }

    public void UnlockLevels(int i){
        int levelToUnlock = levelUnlocked - i*10;
        Button[] levels = levelsParentGameObject[i].GetComponentsInChildren<Button>();
        LevelStars[] stars;
        levelToUnlock = 2; // A retirer et corriger, ne fonctionne plus
                print(levelsUnlock+" "+levelToUnlock);
        for (int j = 0; j < 10; j++)
        {
            if(levelToUnlock > 0){
                levels[j].interactable = true;
                levelToUnlock--;
            }else{
                levels[j].interactable = false;
            }
        }
    }

    public int GetLevelUnlocked(){
        return this.levelUnlocked;
    }
}
