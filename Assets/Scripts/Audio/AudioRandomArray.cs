using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioRandomArray : MonoBehaviour {

    [Range(0f,0.99f)]
    public float randomPitchAmount;
    public AudioClip[] clips;
    public AudioMixerGroup output;

//    void Update ()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            PlaySound();
//        }
//    }

    public void PlaySound()
    {
        //randomize
        int randomClip = Random.Range(0, clips.Length);

        //create an Audiosource
        AudioSource source = gameObject.AddComponent<AudioSource>();

        //load clip into the audio source
        source.clip = clips[randomClip];

        // set the output fo the audiosource
        source.pitch = Random.Range (1-randomPitchAmount,1+randomPitchAmount);

        //play the clip
        source.Play();
        //destroy the audiosource when done playing
        Destroy(source,clips[randomClip].length);
    }


}
