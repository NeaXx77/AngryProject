using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject WorldsAndLevels = null;
    private void Awake() {
        WorldsAndLevels.SetActive(false);
    }
    private void Start() {
        WorldsAndLevels.SetActive(true);
    }
    public void LoadSelectedScene(string levelName){
        string name = levelName;
        SceneManager.LoadScene(name);
    }
}
