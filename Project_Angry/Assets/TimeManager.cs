using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeSlowFactor;
    public float slowdownTime;
    public float speedUpTimer;
    float deltaTime;
    public static TimeManager instance;
    private bool isSettingScale = false;
    private void Start() {
        if(instance != this){
            instance = this;
        }
        print(Time.fixedDeltaTime);
    }
    public void SlowTime(){
        if(!isSettingScale){
            Time.timeScale -= slowdownTime * Time.deltaTime;

            Time.timeScale = Mathf.Clamp(Time.timeScale,timeSlowFactor,1);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }
    public void SetNormalTime(){
        if(!isSettingScale){
            Time.timeScale += speedUpTimer * Time.deltaTime;

            Time.timeScale = Mathf.Clamp(Time.timeScale,timeSlowFactor,1);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }
    public void SetTimeScaleWithTimer(float scale, float timer){
        StopAllCoroutines();
        isSettingScale = true;
        deltaTime = Time.deltaTime;
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        StartCoroutine(FreezeGame(timer));
    }
    IEnumerator FreezeGame(float timer){
        float t = timer;

        while(t > 0){
            t -= deltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        isSettingScale = false;
        yield return null;
    }
}
