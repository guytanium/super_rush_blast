using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class GameplayPause : MonoBehaviour, IPointerDownHandler
{
    private bool isPaused = false;
    // Use this for initialization
    void Start()
    {
        isPaused = false;
        Debug.Log("pause started"); 
    }

    public void PauseClicked()
    {
        if (isPaused == false)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        PauseClicked();
        Debug.Log("pause clicked");
    }
}
