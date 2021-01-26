using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int levelUnlocked;
    public int[][] starsPerLevel; //nb of stars per level corresponding to the array index
    public int nbCollectable;

    public SaveData(){
        levelUnlocked = LevelUnlockManager._levelManager.levelsUnlock;
        starsPerLevel = new int[4][];
        for (int i = 0; i < starsPerLevel.Length; i++)
        {
            starsPerLevel[i] = new int[10];
            for (int j = 0; j < 10; j++)
            {
                starsPerLevel[i][j] = SaveManager._saveManager.levelsStarsWorld[i][j].GetNbStars();
            }
        }
        nbCollectable = PlayerPrefs.GetInt("Collectable",0);
    }
}
