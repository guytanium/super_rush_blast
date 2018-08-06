using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    public bool continuousPlayer;
    public AudioSource musicToPlay;

    private void Start()
    {
        musicToPlay = gameObject.AddComponent<AudioSource>();
        musicToPlay.Play();
        musicToPlay.loop = true;
    }
}