
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class NextScene : MonoBehaviour, IPointerDownHandler
{

    public string levelName;



    public void OnPointerDown(PointerEventData data)

    {
        NextLevelButton(levelName);
        Debug.Log("next scene clicked");

    }


    public void NextLevelButton(string levelName)
    {
        SceneManager.LoadScene(levelName);

        Debug.Log("Changing level to " + levelName);

    }
}
