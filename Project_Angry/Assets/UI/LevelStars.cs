using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStars : MonoBehaviour
{
    private int nbStars = 0;
    public Image starEnabled;
    public Image[] stars = new Image[3];
    public void SetStars(int nbStars){
        this.nbStars = nbStars;
        for (int i = 0; i < nbStars; i++)
        {
            this.stars[i] = starEnabled;
        }
    }
    public int GetNbStars(){
        return nbStars;
    }
}
