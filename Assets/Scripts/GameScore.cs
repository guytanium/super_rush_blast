using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScore : MonoBehaviour
{

    public static int score;
    public string nextLevelName;
    public int levelEndScore;
    // Use this for initialization
    void Start()
    {

    }

    public static void AddPoint(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log("score is: " + score);
    }
    // Update is called once per frame
    void Update()
    {
        if (score >= levelEndScore)
        {
            SceneManager.LoadScene(nextLevelName);
        }



    }
}


